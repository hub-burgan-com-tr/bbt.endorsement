using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
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
