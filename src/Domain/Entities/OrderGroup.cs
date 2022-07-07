using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("OrderGroup", Schema = "order")]
    public class OrderGroup : AuditableEntity
    {
        [Key]
        [MaxLength(36)]
        public string OrderGroupId { get; set; }

        public bool IsCompleted { get; set; }

        public virtual ICollection<OrderMap> OrderMaps { get; set; }
    }
}
