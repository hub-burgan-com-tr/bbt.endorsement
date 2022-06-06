using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
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
        public string Description { get; set; }
        [Required]
        public bool IsStaff { get; set; }
        [MaxLength(4000)]
        public string Response { get; set; }
        [MaxLength(4000)]
        public string Request { get; set; }
        public virtual Document Document { get; set; }
        public virtual Order Order { get; set; }
    }
}
