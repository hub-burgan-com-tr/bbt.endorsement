using Worker.App.Domain.Common;
using Worker.App.Domain.Entities;

namespace Worker.App.Domain.Events.Approvals
{
    public class OrderCreateEvent : DomainEvent
    {
        public OrderCreateEvent(Order order)
        {
            Order = order;
        }

        public Order Order { get; }
    }
}
