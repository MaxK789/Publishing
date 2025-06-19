using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Publishing.Core.Interfaces;
using Publishing.Core.Services;
using Publishing.Infrastructure;
using Publishing.Infrastructure.Repositories;
using Publishing.Infrastructure.DataAccess;

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
            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddSingleton<IPrinteryRepository, PrinteryRepository>();
            services.AddSingleton<ILogger, LoggerService>();
            services.AddSingleton<IPriceCalculator, PriceCalculator>();
            services.AddSingleton<IOrderValidator, OrderValidator>();
            services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
            services.AddSingleton<IDatabaseClient, Publishing.Infrastructure.DataAccess.DatabaseClient>();
            services.AddSingleton<ILoginRepository, LoginRepository>();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<loginForm>();
            services.AddTransient<registrationForm>();
        }
    }
}
