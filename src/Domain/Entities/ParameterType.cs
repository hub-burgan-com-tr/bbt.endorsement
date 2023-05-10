using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("ParameterType", Schema = "parameter")]

    public class ParameterType : AuditableEntity
    {
        public ParameterType()
        {
            Parameters = new HashSet<Parameter>();
        }

        [MaxLength(36)]
        public string ParameterTypeId { get; set; }
        [MaxLength(250)]
        public string Name { get; set; }
        public virtual ICollection<Parameter> Parameters { get; set; }

    }
}
