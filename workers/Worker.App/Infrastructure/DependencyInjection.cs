using Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Worker.App.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCustomDbContext(configuration);
            services.AddCustomScoped();
            services.AddCustomTransien();
            services.AddCustomSingleton();

            return services;
        }
    }
}