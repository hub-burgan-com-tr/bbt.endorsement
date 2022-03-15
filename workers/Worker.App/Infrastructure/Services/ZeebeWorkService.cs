using Microsoft.Extensions.Hosting;
using Serilog;

namespace Worker.App.Infrastructure.Services
{
    public class ZeebeWorkService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Log.Debug("ZeebeWorkService is starting.");


            while (!stoppingToken.IsCancellationRequested)
            {
                Log.Debug("ZeebeWorkService background task is doing background work.");

                await Task.Delay(1000, stoppingToken);
            }

            Log.Debug("ZeebeWorkService background task is stopping.");
        }
    }
}
