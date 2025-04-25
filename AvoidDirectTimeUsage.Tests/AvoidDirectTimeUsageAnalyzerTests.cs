using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvoidDirectTimeUsage.Tests;

[TestClass]
public class AvoidDirectTimeUsageAnalyzerTests
{
    [TestMethod]
    public async Task AvoidDateTime_Now_Diagnostic()
    {
        var testCode = /* lang=c#-test */ """
            using System;
            class C
            {
                void M()
                {
                    var x = [|DateTime.Now|];
                }
            }
            """;

        await new CSharpAnalyzerTest<AvoidDirectTimeUsageAnalyzer, DefaultVerifier>
        {
            ReferenceAssemblies = ReferenceAssemblies.Net.Net80,
            TestCode = testCode
        }.RunAsync();
    }

    [TestMethod]
    public async Task AvoidDateTime_UtcNow_Diagnostic()
    {
        var testCode = /* lang=c#-test */ """
            using System;
            class C
            {
                void M()
                {
                    var x = [|DateTime.UtcNow|];
                }
            }
            """;

        await new CSharpAnalyzerTest<AvoidDirectTimeUsageAnalyzer, DefaultVerifier>
        {
            ReferenceAssemblies = ReferenceAssemblies.Net.Net80,
            TestCode = testCode
        }.RunAsync();
    }

    [TestMethod]
    public async Task AvoidDateTimeOffset_Now_Diagnostic()
    {
        var testCode = /* lang=c#-test */ """
            using System;
            class C
            {
                void M()
                {
                    var x = [|DateTimeOffset.Now|];
                }
            }
            """;

        await new CSharpAnalyzerTest<AvoidDirectTimeUsageAnalyzer, DefaultVerifier>
        {
            ReferenceAssemblies = ReferenceAssemblies.Net.Net80,
            TestCode = testCode
        }.RunAsync();
    }

    [TestMethod]
    public async Task AvoidDateTimeOffset_UtcNow_Diagnostic()
    {
        var testCode = /* lang=c#-test */ """
            using System;
            class C
            {
                void M()
                {
                    var x = [|DateTimeOffset.UtcNow|];
                }
            }
            """;

        await new CSharpAnalyzerTest<AvoidDirectTimeUsageAnalyzer, DefaultVerifier>
        {
            ReferenceAssemblies = ReferenceAssemblies.Net.Net80,
            TestCode = testCode
        }.RunAsync();
    }
}
