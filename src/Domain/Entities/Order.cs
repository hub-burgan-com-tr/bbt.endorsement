using Domain.Common;
using Domain.Events.Approvals;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Order : AuditableEntity, IHasDomainEvent
    {
        [MaxLength(36)]
       
        public string Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string Title { get; set; }


        private bool _done;
        public bool Done
        {
            get => _done;
            set
            {
                if (value == true && _done == false)
                {
                    DomainEvents.Add(new OrderCreateEvent(this));
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