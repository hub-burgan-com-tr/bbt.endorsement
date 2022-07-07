using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("DocumentInsuranceType", Schema = "order")]
    public class DocumentInsuranceType
    {
        [Key]
        [MaxLength(36)]
        public string DocumentInsuranceTypeId { get; set; }

        [MaxLength(36)]
        public string DocumentId { get; set; }
        [MaxLength(36)]

        public string ParameterId { get; set; }


        public virtual Document Document { get; set; }
        public virtual Parameter Parameter { get; set; }
    }
}
