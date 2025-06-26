using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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
}
