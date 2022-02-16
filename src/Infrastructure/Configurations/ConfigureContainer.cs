using Infrastructure.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Configurations;

public static class ConfigureContainer
{
    public static void ConfigureLog(this WebApplicationBuilder builder)
    {
        builder.WebHost.UseClassifiedAdsLogger(configuration =>
        {
            var appSettings = new AppSettings();
            configuration.Bind(appSettings);
            return appSettings.Logging;
        });
    }
}