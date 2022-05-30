using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("DocumentDms", Schema = "order")]
    public class DocumentDms : AuditableEntity
    {
        [Key]
        [MaxLength(36)]
        public string DocumentDmsId { get; set; }
        [MaxLength(36)]
        public string DocumentId { get; set; }
        [MaxLength(36)]
        public string DmsReferenceId { get; set; }

        public virtual Document Document { get; set; }
    }
}
