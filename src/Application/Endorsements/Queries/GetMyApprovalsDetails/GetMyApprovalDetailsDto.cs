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
        public List<Action> Actions { get; set; }


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
        public string Description { get; set; }
        /// <summary>
        /// İşlem Tarihi
        /// </summary>
        public string CreatedDate { get; set; }

    }
}
