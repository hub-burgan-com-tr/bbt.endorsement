using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("OrderDefinition", Schema = "parameter")]

    public class OrderDefinition : AuditableEntity
    {     
        [Key]
        [MaxLength(36)]
        public string OrderDefinitionId { get; set; }
       
        [Required]
        [MaxLength(250)]
        public string Title { get; set; }
        [Required]
        public int ExpireInMinutes { get; set; }
        [Required]
        public int RetryFrequence { get; set; }       
        [Required]
        public int MaxRetryCount { get; set; }
        [Required]
        public string ProcessType { get; set; }
        [Required]
        public string StateType { get; set; }

        public virtual ICollection<OrderDefinitionAction> OrderDefinitionActions { get; set; }


    }
}
