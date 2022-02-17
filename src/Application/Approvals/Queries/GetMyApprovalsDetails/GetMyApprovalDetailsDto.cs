namespace Application.Approvals.Queries.GetMyApprovalsDetails
{
    public class GetMyApprovalDetailsDto
    {
        /// <summary>
        /// Onay Ad
        /// </summary>
        public string ApprovalName { get; set; }
        /// <summary>
        /// Belge Onaylı Mı
        /// </summary>
        public bool IsDocumentApproved { get; set; }
        /// <summary>
        /// Belge Icon
        /// </summary>
        public string ApprovalIcon { get; set; }
        /// <summary>
        /// Onaylayan
        /// </summary>
        public string Approver { get; set; }
        /// <summary>
        /// Onaylama Tarihi
        /// </summary>
        public string ApproverDate { get; set; }
    }
}
