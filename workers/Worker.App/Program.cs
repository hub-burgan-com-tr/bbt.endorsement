using Document.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Worker.App.Application;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Extensions;
using Worker.App.Infrastructure;
using Worker.App.Infrastructure.Configuration;
using Worker.App.Infrastructure.Configuration.Options;
using Worker.App.Infrastructure.Services;
using Worker.App.Services;

var builder = WebApplication.CreateBuilder(args);

IWebHostEnvironment environment = builder.Environment;
IConfiguration Configuration;
if (environment.EnvironmentName == "Development")
{
    Configuration = builder
        .Configuration
        .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", false, true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .AddUserSecrets<Program>()
        .Build();
}
else if (environment.EnvironmentName == "Prod")
{
    Configuration = builder
        .Configuration
        .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", false, true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .AddUserSecrets<Program>()
        .Build();
}
else if (environment.EnvironmentName == "Test")
{
    Configuration = builder
        .Configuration
        .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", false, true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .AddUserSecrets<Program>()
        .Build();
}
else
{
    Configuration = builder
        .Configuration
        .AddJsonFile("appsettings.json", true, true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .AddUserSecrets<Program>()
        .Build();
}


Log.Logger = new LoggerConfiguration()
   .ReadFrom.Configuration(Configuration)
   .CreateLogger();
builder.Host.UseSerilog();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);


builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddDmsInfrastructure(builder.Configuration);
builder.Services.AddSingleton<IConfigurationRoot>(provider => builder.Configuration);


builder.Services.AddHostedService<ZeebeWorkService>();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
var settings = builder.Configuration.Get<AppSettings>();

StaticValuesExtensions.SetStaticValues(settings);

var app = builder.Build();

app.AddUseMiddleware();

using (var scope = app.Services.CreateScope())
{
    Log.Information("Worker.App running... - " + environment.EnvironmentName);
    var serviceProvider = scope.ServiceProvider;

    var zeebeService = serviceProvider.GetRequiredService<IZeebeService>();
    if (zeebeService != null)
    {
        zeebeService.Deploy(settings.Zeebe.ModelFilename);
    }

    while (true)
    {
        // open job worker
        using (var signal = new EventWaitHandle(false, EventResetMode.AutoReset))
        {
            var contractApprovalService = serviceProvider.GetRequiredService<IContractApprovalService>();
            if (contractApprovalService != null)
                contractApprovalService.StartWorkers();

            // blocks main thread, so that worker can run
            signal.WaitOne();
        }
    }
}