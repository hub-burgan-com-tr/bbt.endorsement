using Application.Common.Interfaces;
using Infrastructure.Notification.Web.SignalR;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Infrastructure.ZeebeServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configuration;

public static class ConfigureServiceContainer
{
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.Get<Options.AppSettings>();
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
        services.AddSingleton<IServerEventService, ServerEventService>();
        services.AddSingleton<IClientEventService, ClientEventService>();
        services.AddSingleton<IZeebeService, ZeebeService>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddSignalR();
    }
}
