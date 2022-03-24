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
        
        public string Mode { get; set; }
        public string Url { get; set; }

        public virtual Reference Reference { get; set; } = null!;
    }
}
