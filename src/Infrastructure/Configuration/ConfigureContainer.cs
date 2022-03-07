using Infrastructure.Configuration.Options;
using Infrastructure.Logging;
using Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

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
        builder.WebHost.UseMsLogger(configuration =>
        {
            var appSettings = new AppSettings();
            configuration.Bind(appSettings);
            return appSettings.Logging;
        });
    }

    public static void AddUseMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();
    }
}