using Domain.Common;
using Domain.Entities;

namespace Domain.Events.Approvals
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
