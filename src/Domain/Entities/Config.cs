using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Config : AuditableEntity
    {
        [Key]
        public string ApprovalId { get; set; }

        /// <summary>
        ///  emrin geçerlilik süresi dakika olarak tanımlanır. 
        /// </summary>
        public string TimeoutMinutes { get; set; }

        /// <summary>
        /// hatırlatma frekansını belirlemek için kullanılır. 
        /// </summary>
        public string RetryFrequence { get; set; }

        /// <summary>
        /// kullanıcıya kaç defa hatırlatma yapılacağı bilgisini içerir. 
        /// </summary>
        public int MaxRetryCount { get; set; }


        public virtual Approval Approval { get; set; }
    }
}
