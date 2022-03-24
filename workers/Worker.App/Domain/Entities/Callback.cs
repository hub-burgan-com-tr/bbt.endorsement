using Worker.App.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Worker.App.Domain.Entities
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
        public virtual Reference Reference { get; set; } = null!;
    }
}
