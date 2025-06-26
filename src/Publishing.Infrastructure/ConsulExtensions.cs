using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Publishing.Infrastructure;

public static class ConsulExtensions
{
    public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
    {
        var url = configuration["CONSUL_URL"];
        if (string.IsNullOrWhiteSpace(url))
            throw new InvalidOperationException("CONSUL_URL environment variable is missing");
        services.AddSingleton<IConsulClient>(_ => new ConsulClient(cfg => cfg.Address = new Uri(url)));
        return services;
    }

    public static async Task RegisterWithConsulAsync(this IApplicationBuilder app, IHostApplicationLifetime lifetime, IConfiguration configuration)
    {
        var consul = app.ApplicationServices.GetRequiredService<IConsulClient>();
        var serviceId = $"{configuration["SERVICE_NAME"]}-{Guid.NewGuid()}";

        var tags = configuration["SERVICE_TAGS"]?.Split(',', StringSplitOptions.RemoveEmptyEntries);
        var metaPairs = configuration["SERVICE_META"]?.Split(',', StringSplitOptions.RemoveEmptyEntries);
        Dictionary<string, string>? meta = null;
        if (metaPairs != null && metaPairs.Length > 0)
        {
            meta = new Dictionary<string, string>();
            foreach (var pair in metaPairs)
            {
                var kv = pair.Split('=', 2);
                if (kv.Length == 2)
                    meta[kv[0]] = kv[1];
            }
        }

        var address = configuration["SERVICE_ADDRESS"]!;
        var port = int.Parse(configuration["SERVICE_PORT"] ?? "80");
        var check = new AgentServiceCheck
        {
            HTTP = $"http://{address}:{port}/health",
            Interval = TimeSpan.FromSeconds(10),
            DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(30)
        };

        var registration = new AgentServiceRegistration
        {
            ID = serviceId,
            Name = configuration["SERVICE_NAME"],
            Address = address,
            Port = port,
            Tags = tags,
            Meta = meta,
            Checks = new[] { check }
        };

        await consul.Agent.ServiceRegister(registration);
        lifetime.ApplicationStopping.Register(() => consul.Agent.ServiceDeregister(registration.ID).GetAwaiter().GetResult());
    }
}
