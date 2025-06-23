using System;
using Microsoft.Extensions.DependencyInjection;

namespace Publishing.Services
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUiNotifier(this IServiceCollection services)
        {
            var disabled = Environment.GetEnvironmentVariable("NOTIFICATIONS_DISABLED");
            if (bool.TryParse(disabled?.Trim(), out var isDisabled) && isDisabled)
            {
                services.AddSingleton<IUiNotifier, SilentUiNotifier>();
                return services;
            }

            AddUiNotifierPlatform(services);
            return services;
        }

        static partial void AddUiNotifierPlatform(IServiceCollection services);
    }
}
