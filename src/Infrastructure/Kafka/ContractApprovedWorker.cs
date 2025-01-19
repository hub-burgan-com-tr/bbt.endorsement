using Application.Common.Interfaces;
using Infrastructure.Kafka.SettingModel;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;

namespace Infrastructure.Kafka
{
    public class ContractApprovedWorker : BackgroundService
    {
        private ILogger<ContractApprovedConsumer> _contractApprovedLogger;
        private IOptions<ContractApprovedSettings> _contractApprovedSettings;
        private IApplicationDbContext _context;
        private ISender _mediator;

        public ContractApprovedWorker(ILogger<ContractApprovedConsumer> contractApprovedLogger, IOptions<ContractApprovedSettings> contractApprovedSettings,
        IApplicationDbContext context, ISender mediator)
        {
            _contractApprovedLogger = contractApprovedLogger;
            _contractApprovedSettings = contractApprovedSettings;
            _context = context;
            _mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Log.Information("ContractApproved worker executed.");
            var consumer = new ContractApprovedConsumer(_contractApprovedSettings.Value, stoppingToken, _contractApprovedLogger, _context, _mediator);
            await consumer.ConsumeAsync();
            Log.Information("ContractApproved worker started consume.");
        }
    }
}