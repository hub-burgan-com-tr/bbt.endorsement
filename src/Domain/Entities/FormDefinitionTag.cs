using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("FormDefinitionTag", Schema = "form")]
    public class FormDefinitionTag : AuditableEntity
    {
        [Key]
        [MaxLength(36)]
        public string FormDefinitionTagId { get; set; }

        [MaxLength(36)]
        public string FormDefinitionId { get; set; }
        public string Tag { get; set; }

        public virtual FormDefinition FormDefinition { get; set; }
    }
}
