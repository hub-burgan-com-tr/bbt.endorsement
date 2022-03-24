using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Worker.App.Domain.Common;

namespace Worker.App.Domain.Entities
{
    [Table("DocumentAction", Schema = "order")]

    public class DocumentAction : AuditableEntity
    {
        [Key]
        [MaxLength(36)]
        public string DocumentActionId { get; set; }
        [MaxLength(36)]
        public string DocumentId { get; set; }
        [MaxLength(250)]
        public string Title { get; set; }
        [MaxLength(50)]
        public string Type { get; set; }
        [MaxLength(50)]
        public string State { get; set; }
        public bool IsDefault { get; set; }
        public virtual Document Document { get; set; }

    }
}
