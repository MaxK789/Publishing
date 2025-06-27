using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using ApiGateway;

namespace Publishing.E2E.Tests;

public class AggregationE2ETests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AggregationE2ETests(WebApplicationFactory<Program> factory)
    {
        Environment.SetEnvironmentVariable("CONSUL_URL", "http://consul");
        _factory = factory.WithWebHostBuilder(builder => { });
    }

    [Fact]
    public async Task AggregationEndpoint_ReturnsSuccess()
    {
        using var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/aggregation/person/1");
        response.EnsureSuccessStatusCode();
    }
}
