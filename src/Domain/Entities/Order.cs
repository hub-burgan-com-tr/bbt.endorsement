﻿using Domain.Common;
using Domain.Events.Approvals;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Order", Schema = "order")]
    public class Order : AuditableEntity
    {
        [Key]
        [MaxLength(36)]
        public string OrderId { get; set; }

        [MaxLength(36)]
        public string PersonId { get; set; }
        [MaxLength(36)]
        public string CustomerId { get; set; }
   

        [MaxLength(36)]
        public string DocumentSystemId { get; set; }
        [Required]
        [MaxLength(250)]
        public string Title { get; set; }
        [MaxLength(50)]
        public string State { get; set; }
       

        public long ProcessInstanceKey { get; set; }


        //private bool _done;
        //public bool Done
        //{
        //    get => _done;
        //    set
        //    {
        //        if (value == true && _done == false)
        //        {
        //            DomainEvents.Add(new OrderCreateEvent(this));
        //        }

        //        _done = value;
        //    }
        //}

       // public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
        public virtual Reference Reference { get; set; }
        public virtual Config Config { get; set; }
        public virtual Person Person { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<OrderHistory> OrderHistories { get; set; }
        public virtual ICollection<OrderMap> OrderMaps { get; set; }

    }
}