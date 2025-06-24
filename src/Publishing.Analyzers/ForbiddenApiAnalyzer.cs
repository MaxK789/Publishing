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
    public const string DiagnosticId = "PUB002";
    public const string WindowsDirectiveId = "PUB003";

    private static readonly DiagnosticDescriptor ApiRule = new(
        DiagnosticId,
        "Forbidden API usage",
        "Do not call '{0}'; use IUiNotifier instead",
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
        ImmutableArray.Create(ApiRule, WindowsRule);

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
            var info = context.SemanticModel.GetSymbolInfo(inv);
            var symbol = (info.Symbol ?? info.CandidateSymbols.FirstOrDefault()) as IMethodSymbol;
            if (symbol == null)
                return;

            if (symbol.ContainingType?.ToDisplayString() == "System.Windows.Forms.MessageBox" && symbol.Name == "Show")
            {
                // Highlight the full member access expression
                var location = inv.Expression.GetLocation();
                context.ReportDiagnostic(Diagnostic.Create(ApiRule, location, "MessageBox.Show"));
            }
            else if (symbol.ContainingType?.ToDisplayString() == "System.Windows.Forms.NotifyIcon" && symbol.Name == "ShowBalloonTip")
            {
                Location location = inv.Expression is MemberAccessExpressionSyntax member
                    ? member.Name.GetLocation()
                    : inv.GetLocation();
                context.ReportDiagnostic(Diagnostic.Create(ApiRule, location, "NotifyIcon.ShowBalloonTip"));
            }
        }
    }

    private static void AnalyzeTree(SyntaxTreeAnalysisContext context)
    {
        var root = context.Tree.GetRoot(context.CancellationToken);
        foreach (var directive in root.DescendantTrivia().Where(t => t.IsKind(SyntaxKind.IfDirectiveTrivia)))
        {
            var ifDir = (IfDirectiveTriviaSyntax)directive.GetStructure();
            if (ifDir != null && ifDir.Condition.ToString().Trim() == "WINDOWS")
            {
                context.ReportDiagnostic(Diagnostic.Create(WindowsRule, directive.GetLocation()));
            }
        }
    }
}
