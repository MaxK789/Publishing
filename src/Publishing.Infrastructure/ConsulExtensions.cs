using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

    public static void RegisterWithConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime, IConfiguration configuration)
    {
        var consul = app.ApplicationServices.GetRequiredService<IConsulClient>();
        var serviceId = $"{configuration["SERVICE_NAME"]}-{Guid.NewGuid()}";
        var registration = new AgentServiceRegistration
        {
            ID = serviceId,
            Name = configuration["SERVICE_NAME"],
            Address = configuration["SERVICE_ADDRESS"],
            Port = int.Parse(configuration["SERVICE_PORT"] ?? "80")
        };
        consul.Agent.ServiceRegister(registration).Wait();
        lifetime.ApplicationStopping.Register(() => consul.Agent.ServiceDeregister(registration.ID).Wait());
    }
}
