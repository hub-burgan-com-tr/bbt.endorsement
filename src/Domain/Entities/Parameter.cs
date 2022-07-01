using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Parameter", Schema = "parameter")]

    public class Parameter : AuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public int ParameterTypeId { get; set; }
        [MaxLength(250)]
        public string Text { get; set; }
        public int? DmsReferenceId { get; set; }
        public int? DmsReferenceKey { get; set; }
        [MaxLength(250)]
        public string DmsReferenceName { get; set; }
        public bool IsProcessNo { get; set; }
        public virtual ParameterType ParameterType { get; set; }

        public virtual ICollection<DocumentInsuranceType> DocumentInsuranceTypes { get; set; }
    }
}
