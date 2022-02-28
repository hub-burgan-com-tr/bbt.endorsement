using Infrastructure.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddDbContext(builder.Configuration);
            services.AddScopedServices();
            services.AddTransientServices();
            services.AddSingletonServices();
            services.AddServices();

            builder.ConfigureLog();
            return services;
        }
    }
}