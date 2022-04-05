using Infrastructure;
using Infrastructure.Configuration.Options;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Respawn;
using Serilog;
using System.IO;
using System.Threading.Tasks;

namespace Application.IntegrationTests;

[SetUpFixture]
public class Testing
{
    private static IConfigurationRoot configuration = null!;
    private static IServiceScopeFactory _scopeFactory = null!;
    private static Checkpoint _checkpoint = null!;
    private static string? _currentUserId;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        var builder = WebApplication.CreateBuilder();
        IWebHostEnvironment environment = builder.Environment;

        var configuration = builder
         .Configuration
         .AddJsonFile($"appsettings.Development.json", false, true)
         .AddEnvironmentVariables()
         .Build();

        Log.Logger = new LoggerConfiguration()
           .ReadFrom.Configuration(configuration)
           .CreateLogger();


        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
        builder.Host.UseSerilog();

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder);

        var app = builder.Build();
        using (var scope = app.Services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            _scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
        }
        // _checkpoint = new Checkpoint();
    }
    private static void EnsureDatabase()
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.Migrate();
    }

    public static async Task ResetState()
    {
       // await _checkpoint.Reset(_configuration.GetConnectionString("DefaultConnection"));
        _currentUserId = null;
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }
}
