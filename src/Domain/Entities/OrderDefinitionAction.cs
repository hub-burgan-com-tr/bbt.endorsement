using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("OrderDefinitionAction", Schema = "parameter")]
    public class OrderDefinitionAction : AuditableEntity
    {
        [Key]
        [MaxLength(36)]
        public string OrderDefinitionActionId { get; set; }
        [MaxLength(36)]
        public string OrderDefinitionId { get; set; }
        [MaxLength(250)]
        public string Title { get; set; }
        [MaxLength(50)]
        public string Type { get; set; }
        [MaxLength(50)]
        public string State { get; set; }
        public int Choice { get; set; }

        public virtual OrderDefinition OrderDefinition { get; set; }
    }
}
