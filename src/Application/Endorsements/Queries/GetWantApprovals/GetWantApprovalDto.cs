namespace Application.Endorsements.Queries.GetWantApprovals
{
    public class GetWantApprovalDto
    {
        /// <summary>
        /// Onay Id
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// Onay Ad
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Onaylayan
        /// </summary>
        public string NameAndSurname { get; set; }
        /// <summary>
        /// İslem No
        /// </summary>
        public string ProcessNo { get; set; }
        /// <summary>
        /// Onay Tarihi
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// Belge Var Mı
        /// </summary>
        public bool IsDocument { get; set; } 
        
        /// <summary>
        /// Onay Durumu
        /// </summary>
        public string State { get; set; }

    }
}
