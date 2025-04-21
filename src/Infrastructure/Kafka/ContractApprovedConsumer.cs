using Application.Common.Interfaces;
using Application.Endorsements.Commands.ApproveOrderDocuments;
using bbt.framework.kafka;
using Infrastructure.Kafka.Model;
using Infrastructure.Kafka.SettingModel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Kafka
{
    public class ContractApprovedConsumer : BaseConsumer<ContractApprovedModel>
    {
        private ILogger<ContractApprovedConsumer> _logger;
        private IApplicationDbContext _context;
        private ISender _mediator;

        public ContractApprovedConsumer(ContractApprovedSettings kafkaSettings, CancellationToken cancellationToken, ILogger<ContractApprovedConsumer> logger, IApplicationDbContext context, ISender mediator)
        : base(kafkaSettings, cancellationToken, logger)
        {
            _logger = logger;
            _context = context;
            _mediator = mediator;
        }

        public override async Task<bool> Process(ContractApprovedModel kafkaModel)
        {
            var documentCodes = String.Join(';', kafkaModel.Data.ApprovedDocuments.Select(x => x.Code));
            var instance = _context.ContractStarts.Where(x => x.ContractInstanceId == kafkaModel.Data.ContractInstanceId && x.ContractDocuments == documentCodes).FirstOrDefault();
            if (instance == null)
            {
                return true;
            }

            _logger.LogInformation("ContractApprovedConsumer receive the message and find instance.");

            var documentCount = _context.Documents.Any(x => x.OrderId == instance.OrderId.ToString());
            if (documentCount)
            {
                _logger.LogInformation("ContractApprovedConsumer process started.");
                var documents = _context.Documents.Where(x => x.OrderId == instance.OrderId.ToString()).ToList();

                List<Domain.Models.ApproveOrderDocument> approveOrderDocuments = documents.Select(document =>
                    new Domain.Models.ApproveOrderDocument
                    {
                        DocumentId = document.DocumentId,
                        ActionId = document.DocumentActions.Where(x => x.Type == "Approve").Select(x => x.DocumentActionId).FirstOrDefault(),
                    }).ToList();

                _ = _mediator.Send(new ApproveOrderDocumentCommand
                {
                    OrderId = instance.OrderId.ToString(),
                    Documents = approveOrderDocuments
                }).Result;
            }

            return true;
        }
    }
}