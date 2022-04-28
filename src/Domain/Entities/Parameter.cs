using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("Parameter", Schema = "parameter")]

    public class Parameter : AuditableEntity
    {
       
        public int Id { get; set; }
        public int ParameterTypeId { get; set; }
        [MaxLength(250)]
        public string Text { get; set; }
        public virtual ParameterType ParameterType { get; set; }



    }
}
