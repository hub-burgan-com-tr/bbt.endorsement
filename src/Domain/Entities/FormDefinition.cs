﻿using Domain.Common;
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
            FormDefinitionTagMaps = new HashSet<FormDefinitionTagMap>();
            FormDefinitionActions = new HashSet<FormDefinitionAction>();
        }

        [Key]
        [MaxLength(36)]
        public string FormDefinitionId { get; set; }
        [MaxLength(36)]
        public string ParameterId { get; set; }
        [MaxLength(36)]
        public string DocumentSystemId { get; set; }

        [MaxLength(250)]
        public string Name { get; set; }
        public string Label { get; set; }
        public string HtmlTemplate { get; set; }

        public string Tags { get; set; }
        [MaxLength(250)]
        public string TemplateName { get; set; }
        [MaxLength(50)]
        public string Type { get; set; }
        [MaxLength(50)]
        public string Mode { get; set; }
        public string Url { get; set; }
        [Required]
        public int ExpireInMinutes { get; set; }
        [Required]
        public int RetryFrequence { get; set; }
        [Required]
        public int MaxRetryCount { get; set; }
        [MaxLength(36)]
        public string DependencyFormId { get; set; }
        
        public bool? DependencyReuse { get; set; }
        [MaxLength(10)]
        public string Source { get; set; }


        public virtual Parameter Parameter { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<FormDefinitionTagMap> FormDefinitionTagMaps { get; set; }
        public virtual ICollection<FormDefinitionAction> FormDefinitionActions { get; set; }
    }
}