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
        public List<Action> Actions { get; set; }       
        public List<GetMyApprovalDetailHistoryDto> History { get; set; }
        public string Title { get; internal set; }
    }
    public class Action
    {
        public int Choice { get; set; }
        public string Title { get; set; }
        public string State { get; set; }
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
        public string Name { get; set; }
        /// <summary>
        /// İşlem Tarihi
        /// </summary>
        public string CreatedDate { get; set; }

    }
}
