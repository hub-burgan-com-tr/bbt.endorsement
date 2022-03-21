using System.ComponentModel.DataAnnotations;
using Worker.App.Domain.Common;

namespace Worker.App.Domain.Entities
{
    public class FormDefinitionTag:AuditableEntity
    {
        [Key]
        [MaxLength(36)]
        public string FormDefinitionTagId { get; set; }
        public string FormDefinitionId { get; set; }
        public string Tag { get; set; }

        public virtual FormDefinition FormDefinition { get; set; }
    }
}
