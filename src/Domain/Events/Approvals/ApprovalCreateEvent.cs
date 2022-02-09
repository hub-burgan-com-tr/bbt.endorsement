using Domain.Common;
using Domain.Entities;

namespace Domain.Events.Approvals
{
    public class ApprovalCreateEvent : DomainEvent
    {
        public ApprovalCreateEvent(Approval approval)
        {
            Approval = approval;
        }

        public Approval Approval { get; }
    }
}
