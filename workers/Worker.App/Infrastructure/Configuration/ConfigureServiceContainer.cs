using Dms.Integration.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Infrastructure.Configuration.Options;
using Worker.App.Infrastructure.InternalsServices;
using Worker.App.Infrastructure.Persistence;
using Worker.App.Infrastructure.Services;
using Worker.App.Infrastructure.Services.ZeebeServices;
using Worker.App.Services;

namespace Infrastructure.Configuration
{
    public static class ConfigureServiceContainer
    {
        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.Get<AppSettings>();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    settings.ConnectionStrings.DefaultConnection,
                    configure =>
                    {
                        configure.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        configure.EnableRetryOnFailure();
                    }));
        }

        public static void AddScopedServices(this IServiceCollection services)
        {
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IDomainEventService, DomainEventService>();
        }

        public static void AddTransientServices(this IServiceCollection services)
        {
            services.AddTransient<IDateTime, DateTimeService>();
        }

        public static void AddSingletonServices(this IServiceCollection services)
        {
            services.AddSingleton<IZeebeService, ZeebeService>();
            services.AddSingleton<IContractApprovalService, ContractApprovalService>();
            services.AddSingleton<IInternalsService, InternalsService>();
            services.AddSingleton<IMessagingService, MessagingService>();
        }
    }
}