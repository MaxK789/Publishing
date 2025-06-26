using Microsoft.VisualStudio.TestTools.UnitTesting;
using PactNet;
using System.Net.Http;
using System.Threading.Tasks;

namespace Publishing.Contracts.Tests;

[TestClass]
public class HealthMetricsContracts
{
    [TestMethod]
    public async Task GatewayHealthEndpoint_VerifyContract()
    {
        var pact = Pact.V3("GatewayHealth", "Consumer", new PactConfig());
        pact
            .UponReceiving("A GET request to /health")
            .WithRequest(HttpMethod.Get, "/health")
            .WillRespond()
            .WithStatus(200);

        await pact.VerifyAsync(async ctx =>
        {
            using var client = new HttpClient { BaseAddress = ctx.MockServerUri };
            var response = await client.GetAsync("/health");
            Assert.IsTrue(response.IsSuccessStatusCode);
        });
    }

    [TestMethod]
    public async Task GatewayMetricsEndpoint_VerifyContract()
    {
        var pact = Pact.V3("GatewayMetrics", "Consumer", new PactConfig());
        pact
            .UponReceiving("A GET request to /metrics")
            .WithRequest(HttpMethod.Get, "/metrics")
            .WillRespond()
            .WithStatus(200);

        await pact.VerifyAsync(async ctx =>
        {
            using var client = new HttpClient { BaseAddress = ctx.MockServerUri };
            var response = await client.GetAsync("/metrics");
            Assert.IsTrue(response.IsSuccessStatusCode);
        });
    }
}

