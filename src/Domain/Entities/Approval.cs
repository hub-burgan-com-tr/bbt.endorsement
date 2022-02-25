using Domain.Common;
using Domain.Events.Approvals;

namespace Domain.Entities
{
    public class Approval : AuditableEntity, IHasDomainEvent
    {
        public string ApprovalId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string InstanceId { get; set; } = null!;

        private bool _done;
        public bool Done
        {
            get => _done;
            set
            {
                if (value == true && _done == false)
                {
                    DomainEvents.Add(new ApprovalCreateEvent(this));
                }

                _done = value;
            }
        }

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
        public virtual Reference Reference { get; set; }
        public virtual Config Config { get; set; }

    }
}