using Microsoft.Extensions.DependencyInjection;

namespace Publishing.Services
{
    public static partial class ServiceCollectionExtensions
    {
        static partial void AddUiNotifierPlatform(IServiceCollection services)
        {
            services.AddSingleton<IUiNotifier, WinFormsUiNotifier>();
        }
    }
}
