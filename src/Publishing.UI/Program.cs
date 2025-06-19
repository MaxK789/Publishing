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
using Microsoft.EntityFrameworkCore;

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
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Global handlers for unhandled exceptions
            Application.ThreadException += (s, e) => ShowGlobalException(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                ShowGlobalException(e.ExceptionObject as Exception);

            var services = new ServiceCollection();
            ConfigureServices(services);
            Services = services.BuildServiceProvider();

            // apply pending migrations
            using (var scope = Services.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<Publishing.Infrastructure.Migrations.PublishingDbContext>();
                ctx.Database.Migrate();
            }

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
            services.AddDbContext<Publishing.Infrastructure.Migrations.PublishingDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IDbContext, SqlDbContext>();
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<INavigationService, NavigationService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddTransient<loginForm>();
            services.AddTransient<registrationForm>();
            services.AddTransient<addOrderForm>();
            services.AddTransient<deleteOrderForm>();
            services.AddTransient<mainForm>();
            services.AddTransient<profileForm>();
            services.AddTransient<organizationForm>();
            services.AddTransient<statisticForm>();
        }

        private static void ShowGlobalException(Exception? ex)
        {
            if (ex == null) return;
            // MessageBoxOptions.ServiceNotification shows the dialog above all windows
            MessageBox.Show(
                ex.ToString(),
                "Unhandled Exception",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.ServiceNotification);
        }
    }
}
