using Application.Common.Interfaces;
using Infrastructure.Kafka.SettingModel;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;

namespace Infrastructure.Kafka
{
    public class ContractDocumentCreatedWorker : BackgroundService
    {
        private ILogger<ContractDocumentCreatedConsumer> _documentCreatedLogger;
        private IOptions<ContractDocumentCreatedSettings> _contractDocumentCreatedSettings;
        private IApplicationDbContext _context;
        private ISender _mediator;

        public ContractDocumentCreatedWorker(ILogger<ContractDocumentCreatedConsumer> documentCreatedLogger, IOptions<ContractDocumentCreatedSettings> contractDocumentCreatedSettings,
        IApplicationDbContext context, ISender mediator)
        {
            _documentCreatedLogger = documentCreatedLogger;
            Log.Information("ContractDocumentCreated worker constructor works.");
            _contractDocumentCreatedSettings = contractDocumentCreatedSettings;
            _context = context;
            _mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Log.Information("ContractDocumentCreated worker executed.");
            var consumer = new ContractDocumentCreatedConsumer(_contractDocumentCreatedSettings.Value, stoppingToken, _documentCreatedLogger, _context, _mediator);
            Log.Information("ContractDocumentCreated worker created consumer.");
            await consumer.ConsumeAsync();
            Log.Information("ContractDocumentCreated worker started consume.");
        }
    }
}