using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Action : AuditableEntity
    {
        [Key]
        [MaxLength(36)]
        public string OrderId { get; set; }
        [MaxLength(250)]
        public string Title { get; set; }
        public int Type { get; set; }
        public bool IsDefault { get; set; }
        public virtual Document Document { get; set; }

    }
}
