using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry.Metrics;
using Serilog;
using Publishing.Infrastructure;
using Publishing.Services;
using Publishing.Core.Interfaces;
using ApiGateway.Extensions;
using Polly;
using Polly.Extensions.Http;
using System.Net;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddControllers();
builder.Services.AddCustomSwagger();
builder.Host.UseSerilog((ctx, cfg) =>
{
    cfg.ReadFrom.Configuration(ctx.Configuration)
       .WriteTo.Console();
    var elastic = ctx.Configuration["ELASTIC_URL"];
    if (!string.IsNullOrWhiteSpace(elastic))
        cfg.WriteTo.Elasticsearch(elastic);
});
builder.Services.AddOcelot(builder.Configuration).AddConsul();
builder.Services.AddConsul(builder.Configuration);
var redisConn = builder.Configuration["REDIS_CONN"];
if (string.IsNullOrWhiteSpace(redisConn))
    throw new InvalidOperationException("REDIS_CONN environment variable is missing");
builder.Services.AddStackExchangeRedisCache(o => o.Configuration = redisConn);
builder.Services.AddHealthChecks()
    .AddUrlGroup(new Uri("http://orders/health"), name: "orders")
    .AddUrlGroup(new Uri("http://profile/health"), name: "profile")
    .AddUrlGroup(new Uri("http://organization/health"), name: "organization")
    .AddRedis(redisConn, name: "redis");
var authority = builder.Configuration["OIDC_AUTHORITY"];
var audience = builder.Configuration["OIDC_AUDIENCE"];
if (string.IsNullOrWhiteSpace(authority) || string.IsNullOrWhiteSpace(audience))
    throw new InvalidOperationException("OIDC environment variables are missing");
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = authority;
        options.Audience = audience;
        options.RequireHttpsMetadata = false;
    });
builder.Services.AddAuthorization();
builder.Services.AddSingleton<IUiNotifier, ConsoleUiNotifier>();
builder.Services.AddScoped<IErrorHandler, ErrorHandler>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IJwtFactory, JwtFactory>();

var fallbackResponse = new HttpResponseMessage(HttpStatusCode.OK)
{
    Content = new StringContent("null")
};

var fallbackPolicy = Policy<HttpResponseMessage>
    .Handle<HttpRequestException>()
    .OrResult(r => !r.IsSuccessStatusCode)
    .FallbackAsync(fallbackResponse);

var bulkheadPolicy = Policy.BulkheadAsync<HttpResponseMessage>(5, 20);

builder.Services.AddHttpClient("orders", c => c.BaseAddress = new Uri("http://orders"))
    .AddPolicyHandler(fallbackPolicy)
    .AddPolicyHandler(bulkheadPolicy)
    .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, i => TimeSpan.FromMilliseconds(200 * Math.Pow(2, i))))
    .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
builder.Services.AddHttpClient("profile", c => c.BaseAddress = new Uri("http://profile"))
    .AddPolicyHandler(fallbackPolicy)
    .AddPolicyHandler(bulkheadPolicy)
    .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, i => TimeSpan.FromMilliseconds(200 * Math.Pow(2, i))))
    .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
builder.Services.AddHttpClient("organization", c => c.BaseAddress = new Uri("http://organization"))
    .AddPolicyHandler(fallbackPolicy)
    .AddPolicyHandler(bulkheadPolicy)
    .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, i => TimeSpan.FromMilliseconds(200 * Math.Pow(2, i))))
    .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
builder.Services.AddOpenTelemetry()
    .ConfigureResource(r => r.AddService(builder.Environment.ApplicationName))
    .WithTracing(b => b
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddJaegerExporter())
    .WithMetrics(b => b
        .AddAspNetCoreInstrumentation()
        .AddRuntimeInstrumentation()
        .AddPrometheusExporter());
builder.Logging.ClearProviders();
builder.Host.UseSerilog();

var app = builder.Build();

app.UseCors();
app.UseExceptionHandling();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.RegisterWithConsul(app.Lifetime, app.Configuration);
await app.UseOcelot();
app.MapHealthChecks("/health");
app.UseOpenTelemetryPrometheusScrapingEndpoint();
app.MapControllers();

await app.RunAsync();

public partial class Program { }
