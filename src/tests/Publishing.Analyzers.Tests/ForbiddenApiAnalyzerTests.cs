using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Testing;
using static Microsoft.CodeAnalysis.Testing.ReferenceAssemblies;
using Publishing.Analyzers;

namespace Publishing.Analyzers.Tests;

[TestClass]
public class ForbiddenApiAnalyzerTests
{
    private static async Task VerifyAsync(string source, params DiagnosticResult[] expected)
    {
#pragma warning disable CS0618
        var test = new CSharpAnalyzerTest<ForbiddenApiAnalyzer, MSTestVerifier>
        {
            TestCode = source,
            ReferenceAssemblies = ReferenceAssemblies.Net.Net60
        };
#pragma warning restore CS0618
        test.SolutionTransforms.Add((solution, projectId) =>
        {
            var options = solution.GetProject(projectId)!.CompilationOptions!;
            return solution.WithProjectCompilationOptions(projectId, options.WithOutputKind(OutputKind.ConsoleApplication));
        });
        test.TestState.AdditionalReferences.Add(typeof(System.Windows.Forms.Form).Assembly);
        test.ExpectedDiagnostics.AddRange(expected);
        await test.RunAsync();
    }

    [TestMethod]
    public async Task MessageBoxShow_ProducesDiagnostic()
    {
        var code = "using System.Windows.Forms; class C{ void M(){ MessageBox.Show(\"x\");}}";
        var expected = new DiagnosticResult(ForbiddenApiAnalyzer.DiagnosticId, DiagnosticSeverity.Error)
            .WithSpan(1, 48, 1, 68);
        await VerifyAsync(code, expected);
    }

    [TestMethod]
    public async Task NotifyIconShowBalloonTip_ProducesDiagnostic()
    {
        var code = "using System.Windows.Forms; class C{ void M(){ new NotifyIcon().ShowBalloonTip(1); }}";
        var expected = new DiagnosticResult(ForbiddenApiAnalyzer.DiagnosticId, DiagnosticSeverity.Error)
            .WithSpan(1, 63, 1, 93);
        await VerifyAsync(code, expected);
    }

    [TestMethod]
    public async Task WindowsDirective_ProducesDiagnostic()
    {
        var code = "class C{ void M(){\n#if WINDOWS\nvar x = 0;\n#endif\n}}";
        var expected = new DiagnosticResult(ForbiddenApiAnalyzer.WindowsDirectiveId, DiagnosticSeverity.Error)
            .WithSpan(2, 1, 2, 12);
        await VerifyAsync(code, expected);
    }

    [TestMethod]
    public async Task DebugDirective_NoDiagnostic()
    {
        var code = "class C{ void M(){\n#if DEBUG\nvar x = 0;\n#endif\n}}";
        await VerifyAsync(code);
    }
}
