using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configuration
{
    public static class ConfigureServiceContainer
    {
        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    configure =>
                    {
                        configure.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        configure.EnableRetryOnFailure();
                    }));
        }
    }
}