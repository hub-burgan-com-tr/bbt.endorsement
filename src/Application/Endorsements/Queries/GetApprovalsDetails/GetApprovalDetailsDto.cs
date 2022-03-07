namespace Application.Endorsements.Queries.GetApprovalsDetails
{
    public class GetApprovalDetailsDto
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
        /// Onay Aciklamasi
        /// </summary>
        public string ApprovalDescription { get; set; }
        /// <summary>
        /// Belge Onaylandı Mı
        /// </summary>
        public bool IsDocumentApproved { get; set; }

    }

   
}
