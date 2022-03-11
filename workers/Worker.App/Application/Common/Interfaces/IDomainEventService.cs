using Worker.App.Domain.Common;

namespace Worker.App.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}

