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
        context.RegisterSyntaxNodeAction(AnalyzeMethod, SyntaxKind.MethodDeclaration);
        context.RegisterSyntaxNodeAction(AnalyzeProgram, SyntaxKind.CompilationUnit);
    }

    private static void AnalyzeMethod(SyntaxNodeAnalysisContext context)
    {
        var method = (MethodDeclarationSyntax)context.Node;
        if (method.Identifier.Text != "ConfigureServices")
            return;

        if (!CallsAddUiNotifier(method, context.SemanticModel))
        {
            context.ReportDiagnostic(Diagnostic.Create(Rule, method.Identifier.GetLocation()));
        }
    }

    private static void AnalyzeProgram(SyntaxNodeAnalysisContext context)
    {
        var root = (CompilationUnitSyntax)context.Node;
        if (!CallsAddUiNotifier(root, context.SemanticModel))
        {
            context.ReportDiagnostic(Diagnostic.Create(Rule, root.GetLocation()));
        }
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
