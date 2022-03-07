namespace Application.Endorsements.Queries.GetWatchApprovals
{
    public class GetWatchApprovalDto
    {
        /// <summary>
        /// Onay Id
        /// </summary>
        public int ApprovalId { get; set; }
        /// <summary>
        /// Onay Ad
        /// </summary>
        public string ApprovalName { get; set; }
        /// <summary>
        /// Onaylayan
        /// </summary>
        public string Approver { get; set; }
        /// <summary>
        /// İslem No
        /// </summary>
        public string TransactionNumber { get; set; }
        /// <summary>
        /// Onay Tarihi
        /// </summary>
        public string ApproverDate { get; set; }
        /// <summary>
        /// Belge Var Mı
        /// </summary>
        public bool IsDocument { get; set; }
        /// <summary>
        /// Belge Icon
        /// </summary>
        public string ApprovalIcon { get; set; }
        /// <summary>
        /// Onay Durumu
        /// </summary>
        public string ApprovalState { get; set; }
    }
}
