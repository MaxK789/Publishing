using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Publishing.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class AddUiNotifierAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "PUB001";
    private static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId,
        "AddUiNotifier missing",
        "services.AddUiNotifier() is not called",
        "Usage",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.RegisterCompilationAction(AnalyzeCompilation);
    }

    private static void AnalyzeCompilation(CompilationAnalysisContext context)
    {
        foreach (var tree in context.Compilation.SyntaxTrees)
        {
            var model = context.Compilation.GetSemanticModel(tree);
            var root = tree.GetRoot(context.CancellationToken);
            if (CallsAddUiNotifier(root, model))
                return;
        }

        var location = context.Compilation.SyntaxTrees.FirstOrDefault()?.GetRoot(context.CancellationToken).GetLocation() ?? Location.None;
        context.ReportDiagnostic(Diagnostic.Create(Rule, location));
    }

    private static bool CallsAddUiNotifier(SyntaxNode node, SemanticModel model)
    {
        foreach (var invocation in node.DescendantNodes().OfType<InvocationExpressionSyntax>())
        {
            var symbol = model.GetSymbolInfo(invocation).Symbol as IMethodSymbol;
            if (symbol == null)
                continue;
            if (symbol.Name == "AddUiNotifier")
                return true;
        }
        return false;
    }
}
