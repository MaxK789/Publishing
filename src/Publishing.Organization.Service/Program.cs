using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text;
using Microsoft.OpenApi.Models;
using System;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry.Metrics;
using Serilog;
using Publishing.Infrastructure;
using MediatR;
using Publishing.AppLayer.Handlers;
using Publishing.AppLayer.Mapping;
using Publishing.Organization.Service.Extensions;
using Microsoft.AspNetCore.Authorization;
using Publishing.Services.Authorization;
using Publishing.Services;
using Publishing.Core.Interfaces;
using Publishing.Core.Services;
using FluentValidation;
using Publishing.Infrastructure.Repositories;
using OpenTelemetry.Instrumentation.Runtime;
using Publishing.AppLayer.Validators;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddConsul(builder.Configuration);
var redisConn = builder.Configuration["REDIS_CONN"];
if (string.IsNullOrWhiteSpace(redisConn))
    throw new InvalidOperationException("REDIS_CONN environment variable is missing");
builder.Services.AddStackExchangeRedisCache(o => o.Configuration = redisConn);
var authority = builder.Configuration["OIDC_AUTHORITY"];
var audience = builder.Configuration["OIDC_AUDIENCE"];
if (string.IsNullOrWhiteSpace(authority) || string.IsNullOrWhiteSpace(audience))
    throw new InvalidOperationException("OIDC environment variables are missing");
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = authority;
    options.Audience = audience;
    options.RequireHttpsMetadata = false;
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", p => p.Requirements.Add(new AdminRequirement()));
    options.AddPolicy("RequireContact", p => p.Requirements.Add(new ContactPersonRequirement()));
    options.AddPolicy("RequireStatistics", p => p.Requirements.Add(new StatisticsViewerRequirement()));
});
builder.Services.AddSingleton<IAuthorizationHandler, AdminHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ContactPersonHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, StatisticsViewerHandler>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateOrderHandler).Assembly));
builder.Services.AddAutoMapper(typeof(OrderProfile).Assembly);
builder.Services.AddValidatorsFromAssemblyContaining<EmailValidator>();
builder.Services.AddScoped<IOrderInputValidator, OrderInputValidator>();
builder.Services.AddTransient<PhoneFaxValidator>();
builder.Services.AddTransient<IValidator<string>, EmailValidator>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IDbConnectionFactory, SqlDbConnectionFactory>();
builder.Services.AddTransient<IDbContext, DapperDbContext>();
builder.Services.AddScoped<IDbHelper, DbHelper>();
builder.Services.AddScoped<Publishing.Core.Interfaces.ILogger, LoggerService>();
builder.Services.AddSingleton<IUiNotifier, ConsoleUiNotifier>();
builder.Services.AddScoped<IErrorHandler, ErrorHandler>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IJwtFactory, JwtFactory>();
builder.Services.AddScoped<IPrinteryRepository, PrinteryRepository>();
builder.Services.AddScoped<IDiscountPolicy, StandardDiscountPolicy>();
builder.Services.AddScoped<IPriceCalculator, PriceCalculator>();
builder.Services.AddScoped<IDateTimeProvider, SystemDateTimeProvider>();
var rabbitConn = builder.Configuration["RABBIT_CONN"];
if (string.IsNullOrWhiteSpace(rabbitConn))
{
    builder.Services.AddSingleton<IOrderEventsPublisher, OrderEventsPublisher>();
    builder.Services.AddSingleton<IOrganizationEventsPublisher, OrganizationEventsPublisher>();
    builder.Services.AddSingleton<IProfileEventsPublisher, ProfileEventsPublisher>();
}
else
{
    builder.Services.AddSingleton<IOrderEventsPublisher>(sp =>
        new RabbitOrderEventsPublisher(rabbitConn));
    builder.Services.AddSingleton<IOrganizationEventsPublisher>(sp =>
        new RabbitOrganizationEventsPublisher(rabbitConn));
    builder.Services.AddSingleton<IProfileEventsPublisher>(sp =>
        new RabbitProfileEventsPublisher(rabbitConn));
}
builder.Services.AddOpenTelemetry()
    .ConfigureResource(r => r.AddService(builder.Environment.ApplicationName))
    .WithTracing(b => b
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddEntityFrameworkCoreInstrumentation()
        .AddJaegerExporter())
    .WithMetrics(b => b
        .AddAspNetCoreInstrumentation()
        .AddRuntimeInstrumentation()
        .AddPrometheusExporter());
builder.Logging.ClearProviders();
builder.Host.UseSerilog();

var conn = builder.Configuration["ORGANIZATION_DB_CONN"];
if (string.IsNullOrWhiteSpace(conn))
    throw new InvalidOperationException("ORGANIZATION_DB_CONN environment variable is missing");
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(conn, b =>
        b.MigrationsAssembly("Publishing.Infrastructure")
         .EnableRetryOnFailure()));

builder.Services
    .AddHealthChecks()
    .AddInfrastructureHealthChecks(redisConn, null);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseExceptionHandling();
app.UseAuthentication();
app.UseAuthorization();
if (!app.Environment.IsEnvironment("Test"))
    await app.RegisterWithConsulAsync(app.Lifetime, app.Configuration);
app.MapControllers();

// Map health check endpoint so Docker can check container status
app.MapHealthChecks("/health");
app.UseOpenTelemetryPrometheusScrapingEndpoint();

await app.RunAsync();
