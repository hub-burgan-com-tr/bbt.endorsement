namespace Application.Endorsements.Queries.GetMyApprovalsDetails
{
    public class GetMyApprovalDetailsDto
    {
    
        /// <summary>
        /// Onay Başlık
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Belge Onaylandı Mı
        /// </summary>
        public List<Action> HTMLActions { get; set; }
        public List<Action> PlainTextActions { get; set; }
        public List<Action> PDFActions { get; set; }        
        public List<GetMyApprovalDetailHistoryDto> History { get; set; }

    }
    public class Action
    {
        public bool IsDefault { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }

    }
    public class GetMyApprovalDetailHistoryDto
    {
        /// <summary>
       ///İşlem Ad 
       /// </summary>
        public string State { get; set; }
        /// <summary>
        /// Belge
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// İşlem Tarihi
        /// </summary>
        public string CreatedDate { get; set; }

    }
}
