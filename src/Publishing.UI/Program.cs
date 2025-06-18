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
            using var provider = services.BuildServiceProvider();

            var form = ActivatorUtilities.CreateInstance<loginForm>(provider);
            Application.Run(form);
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddSingleton<IPrinteryRepository, PrinteryRepository>();
            services.AddSingleton<ILogger, LoggerService>();
            services.AddSingleton<IPriceCalculator, PriceCalculator>();
            services.AddSingleton<IOrderValidator, OrderValidator>();
            services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
            services.AddSingleton<ILoginRepository, LoginRepository>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<loginForm>();
        }
    }
}
