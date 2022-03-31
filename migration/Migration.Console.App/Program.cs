using Application.Common.Interfaces;
using Infrastructure.Configuration.Options;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Migration.Console.App;

var builder = WebApplication.CreateBuilder(args);

IWebHostEnvironment environment = builder.Environment;
var configuration = builder
        .Configuration
        .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", false, true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .AddUserSecrets<Program>()
        .Build();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var settings = configuration.Get<AppSettings>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        settings.ConnectionStrings.DefaultConnection,
        configure =>
        {
            configure.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
            configure.EnableRetryOnFailure();
        }));


builder.Services.AddScoped<IMigrationService, MigrationService>();


var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var migrationService = serviceProvider.GetRequiredService<IMigrationService>();
    if (migrationService != null)
    {
        migrationService.Migrate();
    }
}