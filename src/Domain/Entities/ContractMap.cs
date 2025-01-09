using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("ContractMap", Schema = "order")]
    public class ContractMap
    {
        public Guid ContractMapId { get; set; }
        public bool RequiresFullMatch { get; set; }
        public string ContractCode { get; set; }
        public string Language { get; set; }
        public string DocumentCode { get; set; }
        public string DocumentVersion { get; set; }
        public string EndorsementCode { get; set; }
    }
}