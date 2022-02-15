using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Worker.App.Services
{
    public class ZeebeWorkService : BackgroundService
    {
        private readonly ILogger<ZeebeWorkService> _logger;


        public ZeebeWorkService(ILogger<ZeebeWorkService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("ZeebeWorkService is starting.");


            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("ZeebeWorkService background task is doing background work.");

                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogDebug("ZeebeWorkService background task is stopping.");
        }
    }
}
