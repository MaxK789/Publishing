using Microsoft.EntityFrameworkCore;
using Publishing.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using System;
using OpenTelemetry.Trace;
using MediatR;
using Publishing.AppLayer.Handlers;
using Publishing.Profile.Service.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCustomSwagger();
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
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
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
        ValidateLifetime = true
    };
});
builder.Services.AddAuthorization();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateOrderHandler).Assembly));
builder.Services.AddOpenTelemetry().WithTracing(b =>
    b.AddAspNetCoreInstrumentation()
     .AddHttpClientInstrumentation()
     .AddEntityFrameworkCoreInstrumentation()
     .AddConsoleExporter());

builder.Logging.ClearProviders();
builder.Logging.AddJsonConsole();

var conn = builder.Configuration["DB_CONN"];
if (string.IsNullOrWhiteSpace(conn))
    throw new InvalidOperationException("DB_CONN environment variable is missing");
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(conn, b => b.MigrationsAssembly("Publishing.Infrastructure")));

builder.Services
    .AddHealthChecks()
    .AddDatabaseHealthChecks();

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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

await app.RunAsync();
