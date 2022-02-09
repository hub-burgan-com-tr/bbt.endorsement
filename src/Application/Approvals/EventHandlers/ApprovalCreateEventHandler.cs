using Application.Common.Models;
using Domain.Events.Approvals;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Approvals.EventHandlers
{
    public class ApprovalCreateEventHandler : INotificationHandler<DomainEventNotification<ApprovalCreateEvent>>
    {
        private readonly ILogger<ApprovalCreateEventHandler> _logger;

        public ApprovalCreateEventHandler(ILogger<ApprovalCreateEventHandler> logger)
        {
            _logger = logger;
        }


        public Task Handle(DomainEventNotification<ApprovalCreateEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;
            _logger.LogInformation("Domain Event: {DomainEvent}", domainEvent.GetType().Name);
            return Task.CompletedTask;
        }
    }
}
