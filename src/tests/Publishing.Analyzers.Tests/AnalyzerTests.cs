using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using static Microsoft.CodeAnalysis.Testing.ReferenceAssemblies;
using Publishing.Analyzers;

namespace Publishing.Analyzers.Tests;

[TestClass]
public class AnalyzerTests
{
    private static async Task VerifyAsync<T>(string source) where T : DiagnosticAnalyzer, new()
    {
#pragma warning disable CS0618 // MSTestVerifier is obsolete, but newer alternatives are not available in offline builds
        var test = new CSharpAnalyzerTest<T, MSTestVerifier>
        {
            TestCode = source,
            ReferenceAssemblies = ReferenceAssemblies.Net.Net60
        };
#pragma warning restore CS0618

        test.SolutionTransforms.Add((solution, projectId) =>
        {
            var compilationOptions = solution.GetProject(projectId)!.CompilationOptions!
                .WithOutputKind(OutputKind.ConsoleApplication);
            return solution.WithProjectCompilationOptions(projectId, compilationOptions);
        });

        test.TestState.AdditionalReferences.Add(typeof(Microsoft.Extensions.DependencyInjection.ServiceCollection).Assembly);
        test.TestState.AdditionalReferences.Add(typeof(Microsoft.AspNetCore.Builder.WebApplication).Assembly);
        test.TestState.AdditionalReferences.Add(typeof(System.Windows.Forms.Form).Assembly);
        test.TestState.AdditionalReferences.Add(typeof(Publishing.Services.IUiNotifier).Assembly);
        await test.RunAsync();
    }

    [TestMethod]
    public async Task ConfigureServicesWithoutCall_ProducesWarning()
    {
        var code = @"using Microsoft.Extensions.DependencyInjection;
class Startup{void ConfigureServices(IServiceCollection s){}}";
        await VerifyAsync<AddUiNotifierAnalyzer>(code);
    }

    [TestMethod]
    public async Task ConfigureServicesWithCall_NoWarning()
    {
        var code = @"using Microsoft.Extensions.DependencyInjection;
class Startup{void ConfigureServices(IServiceCollection s){s.AddUiNotifier();}}";
        await VerifyAsync<AddUiNotifierAnalyzer>(code);
    }

    [TestMethod]
    public async Task ProgramWithoutCall_ProducesDiagnostic()
    {
        var code = "using Microsoft.Extensions.DependencyInjection;\nusing Microsoft.AspNetCore.Builder;\nvar builder = WebApplication.CreateBuilder();";
        await VerifyAsync<AddUiNotifierAnalyzer>(code);
    }

    [TestMethod]
    public async Task MessageBoxShow_ProducesDiagnostic()
    {
        var code = "using System.Windows.Forms; class C{ void M(){ MessageBox.Show(\"x\");}}";
        await VerifyAsync<ForbiddenApiAnalyzer>(code);
    }

    [TestMethod]
    public async Task BuilderServicesVariable_NoDiagnostic()
    {
        var code = @"using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
var builder = WebApplication.CreateBuilder();
var svcs = builder.Services;
svcs.AddLogging();
svcs.AddUiNotifier();";
        await VerifyAsync<AddUiNotifierAnalyzer>(code);
    }

    [TestMethod]
    public async Task MissingInChainedSetup_ProducesDiagnostic()
    {
        var code = @"using Microsoft.Extensions.DependencyInjection;
class Startup{void ConfigureServices(IServiceCollection s){s
    .AddLogging()
    .AddRouting();}}";
        await VerifyAsync<AddUiNotifierAnalyzer>(code);
    }

    [TestMethod]
    public async Task ExtensionMethodRegistration_NoDiagnostic()
    {
        var code = @"using Microsoft.Extensions.DependencyInjection;
static class Ext{public static void Reg(this IServiceCollection s){s.AddUiNotifier();}}
class S{void ConfigureServices(IServiceCollection s){s.Reg();}}";
        await VerifyAsync<AddUiNotifierAnalyzer>(code);
    }
}
