using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Callback", Schema = "order")]
    public class Callback : AuditableEntity
    {
        [Key]
        [MaxLength(36)] 
        public string OrderId { get; set; }
        [MaxLength(50)]
        public string Mode { get; set; }
        public string Url { get; set; }
        public string ApiKey { get; set; }
        public virtual Reference Reference { get; set; } = null!;
    }
}
