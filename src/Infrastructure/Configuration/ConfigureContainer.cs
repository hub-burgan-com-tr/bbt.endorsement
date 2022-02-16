using Infrastructure.Configuration.Options;
using Infrastructure.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

namespace Infrastructure.Configuration;

public static class ConfigureContainer
{
    public static void ConfigureSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

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