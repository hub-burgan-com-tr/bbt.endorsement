using System.ComponentModel.DataAnnotations;
using Worker.App.Domain.Common;

namespace Worker.App.Domain.Entities
{
    public class FormDefinition:AuditableEntity
    {
        [Key]
        [MaxLength(36)]
        public string FormDefinitionId { get; set; }
        [MaxLength(250)]
        public string Name { get; set; }
        public string Label { get; set; }
        public string Tags { get; set; }
        public string TemplateName { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<FormDefinitionTag> FormDefinitionTags { get; set; }
    }
}
