using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("DocumentAction", Schema = "order")]
    public class DocumentAction : AuditableEntity
    {
        [Key]
        [MaxLength(36)]
        public string DocumentActionId { get; set; }
        [MaxLength(250)]
        public string Title { get; set; }
        public string Type { get; set; }
        public string State { get; set; }
        public bool IsDefault { get; set; }
        [MaxLength(36)]
        public string DocumentId { get; set; }
        public virtual Document Document { get; set; }

    }
}