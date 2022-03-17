using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Worker.App.Application;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Infrastructure;
using Worker.App.Infrastructure.Configuration.Options;
using Worker.App.Infrastructure.Services;
using Worker.App.Services;

var builder = WebApplication.CreateBuilder(args);


IWebHostEnvironment environment = builder.Environment;


if (environment.EnvironmentName == "Development")
{
    var configuration = builder
        .Configuration
        .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", false, true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .AddUserSecrets<Program>()
        .Build();

    Log.Logger = new LoggerConfiguration()
       .ReadFrom.Configuration(configuration)
       .CreateLogger();
}
else
{
    var configuration = builder
        .Configuration
        .AddJsonFile("appsettings.json", false, true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .AddUserSecrets<Program>()
        .Build();

    Log.Logger = new LoggerConfiguration()
       .ReadFrom.Configuration(configuration)
       .CreateLogger();
}

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Host.UseSerilog(); 

var app = builder.Build();

Log.Information("Worker.App running... - " + environment.EnvironmentName);


services.AddApplication();
services.AddInfrastructure(builder.Configuration);

services.AddHostedService<ZeebeWorkService>();
services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
var settings = builder.Configuration.Get<AppSettings>();

var serviceProvider = services.BuildServiceProvider();

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