using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worker.App.Domain.Common;

namespace Worker.App.Domain.Entities
{
    [Table("ParameterType", Schema = "order")]

    public class ParameterType : AuditableEntity
    {
        public ParameterType()
        {
            Parameters = new HashSet<Parameter>();
        }
        public int Id { get; set; }
        [MaxLength(250)]
        public string Name { get; set; }
        public virtual ICollection<Parameter> Parameters { get; set; }
    }
}
