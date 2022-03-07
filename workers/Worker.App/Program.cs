
using Application;
using Application.Common.Interfaces;
using Infrastructure;
using Infrastructure.Configuration.Options;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Worker.App.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder);

builder.Services.AddHostedService<ZeebeWorkService>();

var app = builder.Build();

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{environment}.json", true, true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .AddUserSecrets<Program>()
    .Build();

var settings = configuration.Get<AppSettings>();

using (var scope = app.Services.CreateScope())
{
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