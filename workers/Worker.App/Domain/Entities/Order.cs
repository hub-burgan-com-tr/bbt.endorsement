using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Worker.App.Domain.Common;
using Worker.App.Domain.Events.Approvals;

namespace Worker.App.Domain.Entities
{
    [Table("Order", Schema = "order")]

    public class Order : AuditableEntity, IHasDomainEvent
    {
        [Key]
        [MaxLength(36)]
        public string OrderId { get; set; }

        [MaxLength(36)]
        public string ApproverId { get; set; }
        [MaxLength(36)]
        public string CustomerId { get; set; }

        [Required]
        [MaxLength(250)]
        public string Title { get; set; }
        [MaxLength(50)]
        public string State { get; set; }


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
        public virtual Approver Approver { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<OrderHistory> OrderHistories { get; set; }


    }
}