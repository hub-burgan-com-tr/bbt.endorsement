using Dms.Integration.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Document.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDmsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IPdfConverterService, PdfConverterService>();
        services.AddSingleton<ServiceCaller, ServiceCaller>();
        services.AddScoped<IDocumentService, DocumentService>();
        return services;
    }
}