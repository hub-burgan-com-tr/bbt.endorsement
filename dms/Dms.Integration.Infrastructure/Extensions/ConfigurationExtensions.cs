using Microsoft.Extensions.Configuration;

namespace Dms.Integration.Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static string GetDMSServiceUrl(this IConfiguration config)
    {
        return config.GetSection("ServiceEndpoint")["DMSService"];
    }
}
