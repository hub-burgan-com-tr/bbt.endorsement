namespace Application.Endorsements.Queries.GetMyApprovalsDetails
{
    public class GetMyApprovalDetailsDto
    {

        public string Title { get; internal set; }

        public List<OrderDocument> Documents { get; set; }
        public List<GetMyApprovalDetailHistoryDto> History { get; set; }
    }
    public class OrderDocument
    {
        public string Name { get; set; }     
        public Action Actions { get; set; }
        public string Content { get; internal set; }
        public string Type { get; internal set; }
        public bool State { get; internal set; }
        public string MimeType { get; set; }
        public string OrderState { get; internal set; }
        public string FileName { get; set; }
    }
    public class Action
    {
        public string DocumentId { get; set; }
        public string Title { get; set; }
        public bool Checked { get; set; }
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
        public string Description { get; set; }
        /// <summary>
        /// İşlem Tarihi
        /// </summary>
        public string CreatedDate { get; set; }

    }
}
