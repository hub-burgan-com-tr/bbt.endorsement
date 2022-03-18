using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worker.App.Domain.Common;

namespace Worker.App.Domain.Entities
{
    public class Form : AuditableEntity
    {
        [Key]
        [MaxLength(36)]
        public string FormId { get; set; }
        [MaxLength(250)]
        public string Title { get; set; }
        public virtual ICollection<Order> Orders { get; set; }


    }
}
