using Microsoft.EntityFrameworkCore;
using Publishing.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using System;
using OpenTelemetry.Trace;
using MediatR;
using Publishing.AppLayer.Handlers;
using Publishing.AppLayer.Mapping;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Publishing.Orders.Service.Extensions;
using Microsoft.AspNetCore.Authorization;
using Publishing.Services.Authorization;
using Publishing.Services;
using Publishing.Services.Events;
using Publishing.Core.Interfaces;
using Publishing.Core.Services;
using FluentValidation;
using Publishing.Infrastructure.Repositories;
using Publishing.Services.ErrorHandling;
using Publishing.Services.Roles;
using Publishing.Infrastructure;
using Publishing.AppLayer.Validators;

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
            ValidateLifetime = true,
            RoleClaimType = "role"
        };
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
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IDbConnectionFactory, SqlDbConnectionFactory>();
builder.Services.AddTransient<IDbContext, DapperDbContext>();
builder.Services.AddScoped<IDbHelper, DbHelper>();
builder.Services.AddScoped<ILogger, LoggerService>();
builder.Services.AddUiNotifier();
builder.Services.AddScoped<IErrorHandler, ErrorHandler>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IJwtFactory, JwtFactory>();
var rabbitConn = builder.Configuration["RABBIT_CONN"];
if (string.IsNullOrWhiteSpace(rabbitConn))
    builder.Services.AddSingleton<IOrderEventsPublisher, OrderEventsPublisher>();
else
    builder.Services.AddSingleton<IOrderEventsPublisher>(sp =>
        new RabbitOrderEventsPublisher(rabbitConn));
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
app.UseExceptionHandling();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

await app.RunAsync();
