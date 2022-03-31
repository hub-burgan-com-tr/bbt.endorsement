using Application;
using Infrastructure;
using Infrastructure.Configuration;
using Infrastructure.Configuration.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Migration.Console.App;

var builder = WebApplication.CreateBuilder(args);

IWebHostEnvironment environment = builder.Environment;
builder.Configuration
        .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", false, true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .AddUserSecrets<Program>()
        .Build();


builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder);
builder.Services.AddScoped<IMigrationService, MigrationService>();

var app = builder.Build();

app.AddUseMiddleware();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var migrationService = serviceProvider.GetRequiredService<IMigrationService>();
    if (migrationService != null)
    {
        migrationService.Migrate();
    }
}