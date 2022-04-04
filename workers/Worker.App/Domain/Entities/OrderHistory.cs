using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Worker.App.Domain.Common;

namespace Worker.App.Domain.Entities
{
    [Table("OrderHistory", Schema = "order")]

    public class OrderHistory : AuditableEntity
    {
        [Key]
        [MaxLength(36)]
        public string OrderHistoryId { get; set; }
        [MaxLength(36)]
        [Required]
        public string OrderId { get; set; }
        [MaxLength(36)]

        public string DocumentId { get; set; }
        [Required]
        [MaxLength(250)]
        public string State { get; set; }
        [MaxLength(250)]
        [Required]
        public string Name { get; set; }
        public virtual Document Document { get; set; }
        public virtual Order Order { get; set; }
    }
}
