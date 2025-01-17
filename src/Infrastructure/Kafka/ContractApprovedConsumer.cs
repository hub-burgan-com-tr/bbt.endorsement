using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Endorsements.Commands.ApproveOrderDocuments;
using bbt.framework.kafka;
using Infrastructure.Kafka.Model;
using Infrastructure.Kafka.SettingModel;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog;

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
            Log.Information("ContractApprovedConsumer receive the message.");
            var instance = _context.ContractStarts.Where(x => x.ContractInstanceId == kafkaModel.Data.ContractInstanceId).FirstOrDefault();
            if (instance == null)
            {
                return true;
            }

            var documentCount = _context.Documents.Where(x => x.OrderId == instance.OrderId.ToString()).Count();
            if (documentCount > 1)
            {
                var documents = _context.Documents.Where(x => x.OrderId == instance.OrderId.ToString()).ToList();

                List<Domain.Models.ApproveOrderDocument> approveOrderDocuments = documents.Select(document =>
                    new Domain.Models.ApproveOrderDocument
                    {
                        DocumentId = document.DocumentId,
                        ActionId = document.DocumentActions.Where(x => x.Type == "Approve").Select(x => x.DocumentActionId).FirstOrDefault(),
                    }).ToList();

                await _mediator.Send(new ApproveOrderDocumentCommand
                {
                    OrderId = instance.OrderId.ToString(),
                    Documents = approveOrderDocuments
                });
            }

            return true;
        }
    }
}