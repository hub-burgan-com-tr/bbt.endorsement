namespace Application.Endorsements.Queries.GetApprovalsPhysicallyDocumentDetails
{
    public class GetApprovalPhysicallyDocumentDetailsDto
    {
        /// <summary>
        /// Onay Ad
        /// </summary>
        public string ApprovalName { get; set; }
        /// <summary>
        /// Onay Başlık
        /// </summary>
        public string ApprovalTitle { get; set; }
        /// <summary>
        /// Belge Ad
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        /// Belge Link
        /// </summary>
        public string DocumentLink { get; set; }

        /// <summary>
        /// Onay Aciklamasi
        /// </summary>
        public string ApprovalDescription { get; set; }

        /// <summary>
        /// Belge Onaylandı Mı
        /// </summary>
        public bool IsDocumentApproved { get; set; }
    }
   
}
