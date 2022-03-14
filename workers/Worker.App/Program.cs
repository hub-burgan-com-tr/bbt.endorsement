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

string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(AppContext.BaseDirectory))
            .AddJsonFile("appsettings.json", optional: true);
if (environment == "Development")
    builder.AddJsonFile($"appsettings.{environment}.json", true, true);
else
    builder.AddJsonFile("appsettings.json", false, true);

var Configuration = builder
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .AddUserSecrets<Program>()
    .Build();


var services = new ServiceCollection();
services.AddLogging(config =>
{
    config.AddSerilog();
});
services.AddSingleton<IConfiguration>(Configuration);



Log.Logger = new LoggerConfiguration()
   .ReadFrom.Configuration(Configuration)
   .CreateLogger();
Log.Information("Getting the motors running...");


services.AddApplication();
services.AddInfrastructure(Configuration, builder);

services.AddHostedService<ZeebeWorkService>();
services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
var settings = Configuration.Get<AppSettings>();

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