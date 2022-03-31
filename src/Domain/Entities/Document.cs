using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Document", Schema = "order")]
    public class Document : AuditableEntity
    {
        [Key]
        [MaxLength(36)]
        public string DocumentId { get; set; }


        [MaxLength(36)]
        public string OrderId { get; set; }

        [MaxLength(36)]
        public string FormDefinitionId { get; set; }


        [MaxLength(250)]
        public string Name { get; set; }
        public string Content { get; set; }
        [MaxLength(50)]
        public string Type { get; set; }
        [MaxLength(50)]
        public string State { get; set; }
        public virtual FormDefinition FormDefinition { get; set; }
        public virtual Order Order { get; set; }

        public virtual ICollection<DocumentAction> DocumentActions { get; set; }
        public virtual ICollection<OrderHistory> OrderHistories { get; set; }

    }
}