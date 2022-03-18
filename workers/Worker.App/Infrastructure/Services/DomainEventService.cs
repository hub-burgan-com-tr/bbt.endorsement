using MediatR;
using Worker.App.Domain.Common;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;
using Serilog;

namespace Worker.App.Infrastructure.Services;
public class DomainEventService : IDomainEventService
{
    private readonly IPublisher _mediator;

    public DomainEventService(IPublisher mediator)
    {
        _mediator = mediator;
    }

    public async Task Publish(DomainEvent domainEvent)
    {
        Log.Information("Publishing domain event. Event - {event}", domainEvent.GetType().Name);
        await _mediator.Publish(GetNotificationCorrespondingToDomainEvent(domainEvent));
    }

    private INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent)
    {
        return (INotification)Activator.CreateInstance(
            typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent)!;
    }
}