using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("FormDefinitionAction", Schema = "form")]
    public class FormDefinitionAction : AuditableEntity
    {
        [Key]
        [MaxLength(36)]
        public string FormDefinitionActionId { get; set; }
        [MaxLength(250)]
        public string Title { get; set; }
        [MaxLength(50)]
        public string Type { get; set; }
        [MaxLength(50)]
        public string State { get; set; }
        public bool IsDefault { get; set; }
        [MaxLength(36)]
        public string FormDefinitionId { get; set; }
        public virtual FormDefinition FormDefinition { get; set; }
    }
}