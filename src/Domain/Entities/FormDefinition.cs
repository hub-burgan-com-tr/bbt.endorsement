using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("FormDefinition", Schema = "form")]
    public class FormDefinition : AuditableEntity
    {
        public FormDefinition()
        {
            Documents = new HashSet<Document>();
            FormDefinitionTags=new HashSet<FormDefinitionTag>();
            FormDefinitionActions = new HashSet<FormDefinitionAction>();
        }

        [Key]
        [MaxLength(36)]
        public string FormDefinitionId { get; set; }
        [MaxLength(250)]
        public string Name { get; set; }
        public string Label { get; set; }
        public string Tags { get; set; }
        [MaxLength(250)]
        public string TemplateName { get; set; }
        [MaxLength(50)]
        public string Type { get; set; }
        [MaxLength(50)]
        public string Mode { get; set; }
        public string Url { get; set; }
        public int ExpireInMinutes { get; set; }
        [Required]
        public int RetryFrequence { get; set; }
        public int MaxRetryCount { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<FormDefinitionTag> FormDefinitionTags { get; set; }
        public virtual ICollection<FormDefinitionAction> FormDefinitionActions { get; set; }
    }
}