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
    [Table("FormDefinitionTagMap", Schema = "form")]

    public class FormDefinitionTagMap 
    {
        [MaxLength(36)]
        [Key]
        public string FormDefinitionTagMapId { get; set; }
        [MaxLength(36)]
        public string FormDefinitionId { get; set; }
        [MaxLength(36)]
        public string FormDefinitionTagId { get; set; }
        public virtual FormDefinition FormDefinition { get; set; }
        public virtual FormDefinitionTag FormDefinitionTag { get; set; }

    }
}
