using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class FormDefinition : AuditableEntity
    {
        public FormDefinition()
        {
            Documents = new HashSet<Document>();
            FormDefinitionTags=new HashSet<FormDefinitionTag>();
        }

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