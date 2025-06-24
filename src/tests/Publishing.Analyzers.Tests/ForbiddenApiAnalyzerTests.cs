using System.Threading.Tasks;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Analyzers;

namespace Publishing.Analyzers.Tests;

[TestClass]
public class ForbiddenApiAnalyzerTests
{
    private static async Task<ImmutableArray<Diagnostic>> GetDiagnosticsAsync(string source)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(source);
        var references = new MetadataReference[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Windows.Forms.Form).Assembly.Location)
        };

        var compilation = CSharpCompilation.Create(
            "Test",
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.ConsoleApplication));

        var analyzer = new ForbiddenApiAnalyzer();
        var analyzers = ImmutableArray.Create<DiagnosticAnalyzer>(analyzer);
        var compilationWithAnalyzers = compilation.WithAnalyzers(analyzers);
        return await compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync();
    }

    private static async Task VerifyAsync(string source, params (string id, int line, int column)[] expected)
    {
        var diagnostics = await GetDiagnosticsAsync(source);
        Assert.AreEqual(expected.Length, diagnostics.Length, "Unexpected diagnostic count");

        for (int i = 0; i < expected.Length; i++)
        {
            Assert.AreEqual(expected[i].id, diagnostics[i].Id, $"Diagnostic {i} id mismatch");
            var span = diagnostics[i].Location.GetLineSpan();
            Assert.AreEqual(expected[i].line, span.StartLinePosition.Line + 1, $"Diagnostic {i} line mismatch");
            Assert.AreEqual(expected[i].column, span.StartLinePosition.Character + 1, $"Diagnostic {i} column mismatch");
        }
    }

    [TestMethod]
    public async Task MessageBoxShow_ProducesDiagnostic()
    {
        var code = "using System.Windows.Forms; class C{ void M(){ MessageBox.Show(\"x\");}}";
        await VerifyAsync(code, (ForbiddenApiAnalyzer.DiagnosticId, 1, 48));
    }

    [TestMethod]
    public async Task NotifyIconShowBalloonTip_ProducesDiagnostic()
    {
        var code = "using System.Windows.Forms; class C{ void M(){ new NotifyIcon().ShowBalloonTip(1); }}";
        await VerifyAsync(code, (ForbiddenApiAnalyzer.DiagnosticId, 1, 65));
    }

    [TestMethod]
    public async Task WindowsDirective_ProducesDiagnostic()
    {
        var code = "class C{ void M(){\n#if WINDOWS\nvar x = 0;\n#endif\n}}";
        await VerifyAsync(code, (ForbiddenApiAnalyzer.WindowsDirectiveId, 2, 1));
    }

    [TestMethod]
    public async Task DebugDirective_NoDiagnostic()
    {
        var code = "class C{ void M(){\n#if DEBUG\nvar x = 0;\n#endif\n}}";
        await VerifyAsync(code);
    }
}
