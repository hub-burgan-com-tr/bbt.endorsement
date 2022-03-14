using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Config : AuditableEntity
    {

        [Key]
        [MaxLength(36)]
        public string OrderId { get; set; }
        /// <summary>
        ///  emrin geçerlilik süresi dakika olarak tanımlanır. 
        /// </summary>
        [MaxLength(250)]
        public string TimeoutMinutes { get; set; }

        /// <summary>
        /// hatırlatma frekansını belirlemek için kullanılır. 
        /// </summary>
        [Required]
        [MaxLength(250)]
        public string RetryFrequence { get; set; }

        /// <summary>
        /// kullanıcıya kaç defa hatırlatma yapılacağı bilgisini içerir. 
        /// </summary>
        public int MaxRetryCount { get; set; }


        public virtual Order Order { get; set; }
    }
}
