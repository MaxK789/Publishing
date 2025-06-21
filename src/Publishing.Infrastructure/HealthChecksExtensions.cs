using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Publishing.Infrastructure;

public static class HealthChecksExtensions
{
    public static IHealthChecksBuilder AddDatabaseHealthChecks(this IHealthChecksBuilder builder)
    {
        return builder.AddDbContextCheck<AppDbContext>("Database");
    }
}
