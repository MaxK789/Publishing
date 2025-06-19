using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Windows.Forms;
using Publishing.Core.Interfaces;
using Publishing.Core.Services;
using Publishing.Infrastructure;
using Publishing.Infrastructure.Repositories;
using Publishing.Services;

namespace Publishing
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();
            ConfigureServices(services);
            Services = services.BuildServiceProvider();

            var migrator = Services.GetRequiredService<IDatabaseInitializer>();
            migrator.Initialize();

            var form = Services.GetRequiredService<loginForm>();
            Application.Run(form);

            if (Services is IDisposable d)
            {
                d.Dispose();
            }
        }

        public static ServiceProvider Services { get; private set; } = null!;

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IStatisticRepository, StatisticRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IPrinteryRepository, PrinteryRepository>();
            services.AddScoped<ILogger, LoggerService>();
            services.AddScoped<IPriceCalculator, PriceCalculator>();
            services.AddScoped<IOrderValidator, OrderValidator>();
            services.AddScoped<IDateTimeProvider, SystemDateTimeProvider>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            services.AddSingleton<IConfiguration>(configuration);
            services.AddScoped<IDbContext, SqlDbContext>();
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<INavigationService, NavigationService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IDatabaseInitializer, DatabaseMigrator>();
            services.AddTransient<loginForm>();
            services.AddTransient<registrationForm>();
            services.AddTransient<addOrderForm>();
            services.AddTransient<deleteOrderForm>();
            services.AddTransient<mainForm>();
            services.AddTransient<profileForm>();
            services.AddTransient<organizationForm>();
            services.AddTransient<statisticForm>();
        }
    }
}
