namespace Application.Endorsements.Queries.GetApprovalsDocumentList
{
    public class GetApprovalsDocumentListDto
    {
        /// <summary>
        /// OnayAd
        /// </summary>
        public int ApprovalName { get; set; }
        /// <summary>
        /// Belge Ad
        /// </summary>
        public string DocumentName { get; set; }
        /// <summary>
        /// Belge Icon
        /// </summary>
        public string DocumentIcon { get; set; }
        /// <summary>
        /// Belge Onaylandımı
        /// </summary>
        public bool IsDocumentApproved { get; set; }
    }
}
