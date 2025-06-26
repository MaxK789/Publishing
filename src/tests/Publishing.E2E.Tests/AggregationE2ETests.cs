using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Publishing.E2E.Tests;

public class AggregationE2ETests : IClassFixture<WebApplicationFactory<ApiGateway.Program>>
{
    private readonly WebApplicationFactory<ApiGateway.Program> _factory;

    public AggregationE2ETests(WebApplicationFactory<ApiGateway.Program> factory)
    {
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
