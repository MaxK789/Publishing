using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Publishing.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ForbiddenApiAnalyzer : DiagnosticAnalyzer
{
    public const string MessageBoxId = "PUB002";
    public const string WindowsDirectiveId = "PUB003";

    private static readonly DiagnosticDescriptor MessageBoxRule = new(
        MessageBoxId,
        "MessageBox.Show usage",
        "Do not use MessageBox.Show; use IUiNotifier",
        "Usage",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor WindowsRule = new(
        WindowsDirectiveId,
        "#if WINDOWS usage",
        "Do not use #if WINDOWS conditional compilation",
        "Usage",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(MessageBoxRule, WindowsRule);

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
        context.RegisterSyntaxTreeAction(AnalyzeTree);
    }

    private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is InvocationExpressionSyntax inv)
        {
            var symbol = context.SemanticModel.GetSymbolInfo(inv).Symbol as IMethodSymbol;
            if (symbol?.ContainingType?.ToDisplayString() == "System.Windows.Forms.MessageBox" && symbol.Name == "Show")
            {
                context.ReportDiagnostic(Diagnostic.Create(MessageBoxRule, inv.GetLocation()));
            }
        }
    }

    private static void AnalyzeTree(SyntaxTreeAnalysisContext context)
    {
        var root = context.Tree.GetRoot(context.CancellationToken);
        foreach (var directive in root.DescendantTrivia().Where(t => t.IsKind(SyntaxKind.IfDirectiveTrivia))
        {
            var ifDir = (IfDirectiveTriviaSyntax)directive.GetStructure();
            if (ifDir != null && ifDir.Condition.ToString().Contains("WINDOWS"))
            {
                context.ReportDiagnostic(Diagnostic.Create(WindowsRule, directive.GetLocation()));
            }
        }
    }
}
