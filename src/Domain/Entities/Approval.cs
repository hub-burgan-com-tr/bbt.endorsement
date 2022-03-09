using Domain.Common;
using Domain.Events.Approvals;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Approval : AuditableEntity, IHasDomainEvent
    {
        public string ApprovalId { get; set; }
        public string Title { get; set; }
        public long Customer { get; set; }
        public long Approver { get; set; }


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


        public virtual ICollection<Document> Documents { get; set; }

    }
}