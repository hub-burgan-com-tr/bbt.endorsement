using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("ContractStart", Schema = "order")]
    public class ContractStart
    {
        [Key]
        public Guid ContractStartId { get; set; }
        public Guid ContractInstanceId { get; set; }
        public Guid OrderId { get; set; }
        public string ContractDocuments { get; set; }
    }
}