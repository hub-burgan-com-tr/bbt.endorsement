using Document.Infrastructure;
using Elastic.Apm.NetCoreAll;
using Elastic.Apm.SerilogEnricher;
using Elastic.CommonSchema.Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
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
else if (environment.EnvironmentName == "Uat")
{
    Configuration = builder
        .Configuration
        .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", false, true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .AddUserSecrets<Program>()
        .Build();
}
else if (environment.EnvironmentName == "Preprod")
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
//builder.Host.UseSerilog();
builder.Host.UseSerilog((context, services, configuration) => configuration
                    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.WithElasticApmCorrelationInfo()
                    .Enrich.FromLogContext().Enrich.WithEnvironmentName().Enrich.WithMachineName().Enrich.WithProcessId().Enrich.WithThreadId()
                    .WriteTo.Async(e =>
                    {
                        e.Elasticsearch(new ElasticsearchSinkOptions(new Uri(builder.Configuration["ElasticSearchSettings:Uri"]))
                        {
                            ModifyConnectionSettings = x => x.BasicAuthentication(builder.Configuration["ElasticSearchSettings:Username"], builder.Configuration["ElasticSearchSettings:Password"]),
                            AutoRegisterTemplate = true,
                            OverwriteTemplate = true,
                            IndexFormat = builder.Configuration["ElasticSearchSettings:IndexFormat"],
                            MinimumLogEventLevel = LogEventLevel.Information,
                            TypeName = null,
                            BatchPostingLimit = 1,
                            CustomFormatter = new EcsTextFormatter()
                        });
                        e.Console(outputTemplate: "{Level} {ElasticApmTraceId} {ElasticApmTransactionId} {Message:lj:maxlength=10000} {NewLine}{Exception}");
                    }));
AppContext.SetSwitch("System.Globalization.Invariant", false);
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

if (environment.EnvironmentName == "Prod" || environment.EnvironmentName == "Uat")
    app.UseAllElasticApm(Configuration);


app.AddUseMiddleware();

using (var scope = app.Services.CreateScope())
{
    Log.Information("Worker.App running... - " + environment.EnvironmentName);
    var serviceProvider = scope.ServiceProvider;

    var zeebeService = serviceProvider.GetRequiredService<IZeebeService>();
    if (zeebeService != null && (environment.EnvironmentName == "Prod " || environment.EnvironmentName == "Preprod"))
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