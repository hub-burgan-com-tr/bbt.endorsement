using Application.Common.Interfaces;
using Application.Endorsements.Commands.ApproveOrderDocuments;
using bbt.framework.kafka;
using Infrastructure.Kafka.Model;
using Infrastructure.Kafka.SettingModel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Kafka
{
    public class ContractDocumentCreatedConsumer : BaseConsumer<ContractDocumentCreatedModel>
    {
        private ILogger<ContractDocumentCreatedConsumer> _logger;
        private IApplicationDbContext _context;
        private ISender _mediator;

        public ContractDocumentCreatedConsumer(ContractDocumentCreatedSettings kafkaSettings, CancellationToken cancellationToken, ILogger<ContractDocumentCreatedConsumer> logger, IApplicationDbContext context, ISender mediator)
        : base(kafkaSettings, cancellationToken, logger)
        {
            _logger = logger;
            _context = context;
            _mediator = mediator;
        }

        public override async Task<bool> Process(ContractDocumentCreatedModel kafkaModel)
        {
            var instance = _context.ContractStarts.Where(x => x.ContractInstanceId == kafkaModel.Data.ContractInstanceId && x.ContractDocuments == kafkaModel.Data.DocumentCode).FirstOrDefault();
            if (instance == null)
            {
                return true;
            }

            _logger.LogInformation("ContractDocumentCreatedConsumer receive the message and find instance.");

            var documentCount = _context.Documents.Where(x => x.OrderId == instance.OrderId.ToString()).Count();
            if (documentCount == 1)
            {
                _logger.LogInformation("ContractDocumentCreatedConsumer process started.");
                var document = _context.Documents.Where(x => x.OrderId == instance.OrderId.ToString()).FirstOrDefault();

                List<Domain.Models.ApproveOrderDocument> documents = new List<Domain.Models.ApproveOrderDocument>();
                if (kafkaModel.Data.ApprovalStatus == "Approved")
                {
                    documents.Add(new Domain.Models.ApproveOrderDocument
                    {
                        DocumentId = document.DocumentId,
                        ActionId = document.DocumentActions.Where(x => x.Type == "Approve").Select(x => x.DocumentActionId).FirstOrDefault()
                    });

                    _logger.LogInformation("ContractDocumentCreatedConsumer Document Approved.");
                }
                else if (kafkaModel.Data.ApprovalStatus == "Rejected")
                {
                    documents.Add(new Domain.Models.ApproveOrderDocument
                    {
                        DocumentId = document.DocumentId,
                        ActionId = document.DocumentActions.Where(x => x.Type == "Reject").Select(x => x.DocumentActionId).FirstOrDefault()
                    });

                    _logger.LogInformation("ContractDocumentCreatedConsumer Document Rejected.");
                }

                _ = _mediator.Send(new ApproveOrderDocumentCommand
                {
                    OrderId = instance.OrderId.ToString(),
                    Documents = documents
                }).Result;
            }

            return true;
        }
    }
}