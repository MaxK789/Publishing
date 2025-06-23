using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Windows.Forms;
using Publishing.Core.Interfaces;
using Publishing.Core.Services;
using Publishing.AppLayer.Mapping;
using Publishing.Infrastructure;
using Publishing.Infrastructure.Repositories;
using Publishing.Infrastructure.DataAccess;
using Publishing.Infrastructure.Queries;
using Microsoft.Extensions.Caching.Memory;
using MediatR;
using Publishing.AppLayer.Handlers;
using Publishing.AppLayer.Validators;
using Publishing.AppLayer.Behaviors;
using FluentValidation;
using Publishing.Services;
using Publishing.Services.ErrorHandling;
using Publishing.Services.Events;

namespace Publishing
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();
            ConfigureServices(services);
            Services = services.BuildServiceProvider();



            var notifier = Services.GetRequiredService<IUiNotifier>() as IDisposable;
            System.Windows.Forms.Application.ApplicationExit += (_, __) => notifier?.Dispose();

            var form = Services.GetRequiredService<loginForm>();
            System.Windows.Forms.Application.Run(form);

            if (Services is IDisposable d)
            {
                d.Dispose();
            }
        }

        public static ServiceProvider Services { get; private set; } = null!;

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderQueries, OrderQueries>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IStatisticRepository, StatisticRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IPrinteryRepository, PrinteryRepository>();
            services.AddScoped<ILogger, LoggerService>();
            services.AddScoped<IDiscountPolicy, StandardDiscountPolicy>();
            services.AddScoped<IPriceCalculator, PriceCalculator>();
            services.AddScoped<IOrderInputValidator, OrderInputValidator>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddUiNotifier();
            services.AddScoped<IErrorHandler, ErrorHandler>();
            services.AddScoped<IDateTimeProvider, SystemDateTimeProvider>();
            services.AddSingleton<IUserSession, UserSession>();
            services.AddMemoryCache();
            services.AddAutoMapper(typeof(OrderProfile).Assembly);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateOrderHandler).Assembly));
            services.AddValidatorsFromAssemblyContaining<EmailValidator>();
            services.AddTransient<IValidator<string>, EmailValidator>();
            services.AddTransient<PhoneFaxValidator>();
            services.AddTransient<IValidator<string>, PhoneFaxValidator>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            System.Diagnostics.Debug.WriteLine(
                "Connection string: " + configuration.GetConnectionString("DefaultConnection"));

            services.AddSingleton<IConfiguration>(configuration);
            services.AddDbContext<AppDbContext>(o =>
                o.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Publishing.Infrastructure")));
            services.AddTransient<IDbConnectionFactory, SqlDbConnectionFactory>();
            services.AddSingleton<QueryDispatcher>();
            services.AddSingleton<IQueryDispatcher>(sp =>
                new MemoryCacheQueryDispatcher(
                    sp.GetRequiredService<QueryDispatcher>(),
                    sp.GetRequiredService<IMemoryCache>(),
                    TimeSpan.FromMinutes(10)));
            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
            services.AddTransient<IDbContext, DapperDbContext>();
            services.AddScoped<IDbHelper, DbHelper>();
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IJwtFactory, JwtFactory>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<INavigationService, NavigationService>();
            var rabbit = configuration["RABBIT_CONN"];
            if (string.IsNullOrWhiteSpace(rabbit))
                services.AddSingleton<IOrderEventsPublisher, OrderEventsPublisher>();
            else
                services.AddSingleton<IOrderEventsPublisher>(new RabbitOrderEventsPublisher(rabbit));
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
