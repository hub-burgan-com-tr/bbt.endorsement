using Microsoft.AspNetCore.Builder;
using Worker.App.Infrastructure.Middleware;

namespace Worker.App.Infrastructure.Configuration;

public static class ConfigureContainer
{
    public static void AddUseMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();
    }
}