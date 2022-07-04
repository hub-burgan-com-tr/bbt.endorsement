using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("FormDefinitionTag", Schema = "form")]
    public class FormDefinitionTag : AuditableEntity
    {
        public FormDefinitionTag()
        {
            FormDefinitionTagMaps = new HashSet<FormDefinitionTagMap>();
        }
        [Key]
        [MaxLength(36)]
        public string FormDefinitionTagId { get; set; }
        public bool IsProcessNo { get; set; }

       
        public string Tag { get; set; }
        public virtual ICollection<FormDefinitionTagMap> FormDefinitionTagMaps { get; set; }

    }
}
