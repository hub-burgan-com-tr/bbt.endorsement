using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Worker.App.Application;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Infrastructure;
using Worker.App.Infrastructure.Configuration.Options;
using Worker.App.Infrastructure.Services;
using Worker.App.Services;

var builder = WebApplication.CreateBuilder(args);

IWebHostEnvironment environment = builder.Environment;

if (environment.EnvironmentName == "Development")
    builder
        .Configuration
        .AddJsonFile($"appsettings.{environment}.json", true, true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .AddUserSecrets<Program>()
        .Build();
else
    builder
        .Configuration
        .AddJsonFile("appsettings.json", false, true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .AddUserSecrets<Program>()
        .Build();


var services = new ServiceCollection();
services.AddLogging(config =>
{
    config.AddSerilog();
});
services.AddSingleton<IConfiguration>(builder.Configuration);



Log.Logger = new LoggerConfiguration()
   .ReadFrom.Configuration(builder.Configuration)
   .CreateLogger();
Log.Information("Getting the motors running...");


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