namespace Application.Approvals.Queries.GetApprovalsDetails
{
    public class GetApprovalDetailsDto
    {
        /// <summary>
        /// Onay Ad
        /// </summary>
        public string ApprovalName { get; set; }
        /// <summary>
        /// Onay Aciklamasi
        /// </summary>
        public string ApprovalDescription { get; set; }
        /// <summary>
        /// Belge Ad
        /// </summary>
        public string DocumentLink { get; set; }
        /// <summary>
        /// Belge Onaylandı Mı
        /// </summary>
        public bool IsDocumentApproved { get; set; }

    }
}
