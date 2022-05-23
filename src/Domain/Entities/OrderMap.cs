using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("OrderMap", Schema = "order")]
    public class OrderMap : AuditableEntity
    {
        [Key]
        [MaxLength(36)]
        public string OrderMapId { get; set; }
        [MaxLength(36)]
        public string OrderGroupId { get; set; }
        [MaxLength(36)]
        public string OrderId { get; set; }
        [MaxLength(36)]
        public string DocumentId { get; set; }


        public virtual OrderGroup OrderGroup { get; set; }
        public virtual Order Order { get; set; }
        public virtual Document Document { get; set; }
    }
}
