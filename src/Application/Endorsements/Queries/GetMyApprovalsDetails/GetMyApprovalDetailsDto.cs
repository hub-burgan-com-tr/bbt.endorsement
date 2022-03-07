namespace Application.Endorsements.Queries.GetMyApprovalsDetails
{
    public class GetMyApprovalDetailsDto
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
        /// Belge Onaylandı Mı
        /// </summary>
        public bool IsDocumentApproved { get; set; }
        /// <summary>
        /// Belge Icon
        /// </summary>
        public string DocumentIcon { get; set; }
        public List<GetMyApprovalDetailHistoryDto> GetMyApprovalDetailHistory { get; set; }

    }

    public class GetMyApprovalDetailHistoryDto
    {
        /// <summary>
       ///İşlem Ad 
       /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Belge
        /// </summary>
        public string DocumentName { get; set; }
        /// <summary>
        /// İşlem Tarihi
        /// </summary>
        public string CreatedDate { get; set; }

    }
}
