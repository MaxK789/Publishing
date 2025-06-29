using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using ApiGateway;
using System.Net.Http;
using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;
using Publishing.Core.Interfaces;
using CoreLogger = Publishing.Core.Interfaces.ILogger;
using Publishing.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Publishing.Integration.Tests;

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "test") }, Scheme.Name));
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}

[TestClass]
public class GatewayAggregationTests
{
    private WebApplicationFactory<Program> CreateFactory()
    {
        Environment.SetEnvironmentVariable("CONSUL_URL", "http://consul");
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
        Environment.SetEnvironmentVariable("REDIS_CONN", "localhost");
        Environment.SetEnvironmentVariable("OIDC_AUTHORITY", "http://auth");
        Environment.SetEnvironmentVariable("OIDC_AUDIENCE", "aud");
        Environment.SetEnvironmentVariable("ELASTIC_URL", "http://elastic");
        return new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Test");
                builder.ConfigureServices(services =>
                {
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", _ => { });
                    services.AddTransient<CoreLogger, LoggerService>();
                    services.AddHttpClient("orders").ConfigurePrimaryHttpMessageHandler(() => new StubHandler("[]"));
                    services.AddHttpClient("profile").ConfigurePrimaryHttpMessageHandler(() => new StubHandler("{}"));
                    services.AddHttpClient("organization").ConfigurePrimaryHttpMessageHandler(() => new StubHandler("{}"));
                });
            });
    }

    private class StubHandler : HttpMessageHandler
    {
        private readonly string _content;
        public StubHandler(string content) => _content = content;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(_content) });
    }

    [TestMethod]
    public async Task AggregationEndpoint_ReturnsSuccess()
    {
        using var factory = CreateFactory();
        using var client = factory.CreateClient();
        var response = await client.GetAsync("/api/aggregation/person/1");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task MetricsEndpoint_Available()
    {
        using var factory = CreateFactory();
        using var client = factory.CreateClient();
        var response = await client.GetAsync("/metrics");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}
