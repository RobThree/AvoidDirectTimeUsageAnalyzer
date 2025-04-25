using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("AvoidDirectTimeUsage.Tests")]

namespace AvoidDirectTimeUsage;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class AvoidDirectTimeUsageAnalyzer : DiagnosticAnalyzer
{
    internal static readonly DiagnosticDescriptor Rule = new(
        id: "TP0001",
        title: "Avoid direct time access",
        messageFormat: "Use TimeProvider instead of '{0}'",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Accessing DateTime.Now, DateTime.UtcNow, DateTimeOffset.Now or DateTimeOffset.UtcNow reduces testability. Use TimeProvider instead.");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeMemberAccess, SyntaxKind.SimpleMemberAccessExpression);
    }

    private static void AnalyzeMemberAccess(SyntaxNodeAnalysisContext context)
    {
        var memberAccess = (MemberAccessExpressionSyntax)context.Node;

        if (context.SemanticModel.GetSymbolInfo(memberAccess).Symbol is not IPropertySymbol symbol || symbol.ContainingType == null)
        {
            return;
        }

        var typeName = symbol.ContainingType.ToString();
        var propertyName = symbol.Name;

        if ((typeName == "System.DateTime" || typeName == "System.DateTimeOffset") &&
            (propertyName == "Now" || propertyName == "UtcNow"))
        {
            var diagnostic = Diagnostic.Create(Rule, memberAccess.GetLocation(), $"{typeName}.{propertyName}");
            context.ReportDiagnostic(diagnostic);
        }
    }
}
