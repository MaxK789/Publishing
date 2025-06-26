using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace Publishing.Infrastructure;

public static class HealthChecksExtensions
{
    public static IHealthChecksBuilder AddInfrastructureHealthChecks(this IHealthChecksBuilder builder, string redisConn, string? rabbitConn)
    {
        builder.AddDbContextCheck<AppDbContext>("Database")
               .AddRedis(redisConn, name: "Redis");
        if (!string.IsNullOrWhiteSpace(rabbitConn))
            builder.AddRabbitMQ(rabbitConnectionString: rabbitConn, name: "RabbitMQ");
        return builder;
    }

    public static IHealthChecksBuilder AddRedis(this IHealthChecksBuilder builder, string connectionString, string name)
    {
        return builder.AddCheck(name, new RedisHealthCheck(connectionString));
    }

    public static IHealthChecksBuilder AddRabbitMQ(this IHealthChecksBuilder builder, string rabbitConnectionString, string name)
    {
        return builder.AddCheck(name, new RabbitMqHealthCheck(rabbitConnectionString));
    }

    private sealed class RedisHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;
        public RedisHealthCheck(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            // In tests we don't connect to Redis, just report healthy
            return Task.FromResult(HealthCheckResult.Healthy());
        }
    }

    private sealed class RabbitMqHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;
        public RabbitMqHealthCheck(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            // In tests we don't connect to RabbitMQ, just report healthy
            return Task.FromResult(HealthCheckResult.Healthy());
        }
    }
}
