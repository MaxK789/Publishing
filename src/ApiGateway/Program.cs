using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OpenTelemetry.Trace;
using Publishing.Services;
using Publishing.Services.ErrorHandling;
using Publishing.Services.Roles;
using Publishing.Services.Jwt;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddHealthChecks();
var redisConn = builder.Configuration["REDIS_CONN"];
if (string.IsNullOrWhiteSpace(redisConn))
    throw new InvalidOperationException("REDIS_CONN environment variable is missing");
builder.Services.AddStackExchangeRedisCache(o => o.Configuration = redisConn);
var issuer = builder.Configuration["JWT:Issuer"];
var audience = builder.Configuration["JWT:Audience"];
var signingKey = builder.Configuration["JWT:SigningKey"];
if (string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience) || string.IsNullOrWhiteSpace(signingKey))
    throw new InvalidOperationException("JWT environment variables are missing");
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
            ValidateLifetime = true,
            RoleClaimType = "role"
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddScoped<IErrorHandler, ErrorHandler>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IJwtFactory, JwtFactory>();
builder.Services.AddOpenTelemetry().WithTracing(b =>
    b.AddAspNetCoreInstrumentation()
     .AddHttpClientInstrumentation()
     .AddConsoleExporter());
builder.Logging.ClearProviders();
builder.Logging.AddJsonConsole();

var app = builder.Build();

app.UseCors();
app.UseExceptionHandling();
app.UseAuthentication();
app.UseAuthorization();
await app.UseOcelot();
app.MapHealthChecks("/health");

app.Run();
