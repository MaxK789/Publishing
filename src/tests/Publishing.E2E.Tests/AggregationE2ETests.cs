using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using ApiGateway;
using Publishing.Core.Interfaces;
using Publishing.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Publishing.E2E.Tests;

public class AggregationE2ETests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AggregationE2ETests(WebApplicationFactory<Program> factory)
    {
        Environment.SetEnvironmentVariable("CONSUL_URL", "http://consul");
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
        Environment.SetEnvironmentVariable("REDIS_CONN", "localhost");
        Environment.SetEnvironmentVariable("OIDC_AUTHORITY", "http://auth");
        Environment.SetEnvironmentVariable("OIDC_AUDIENCE", "audience");
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Test");
            builder.ConfigureServices(services =>
            {
                services.AddTransient<ILogger, LoggerService>();
            });
        });
    }

    [Fact]
    public async Task AggregationEndpoint_ReturnsSuccess()
    {
        using var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/aggregation/person/1");
        response.EnsureSuccessStatusCode();
    }
}
