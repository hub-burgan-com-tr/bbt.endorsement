namespace Application.Endorsements.Queries.GetApprovalsPhysicallyDocumentDetails
{
    public class GetApprovalPhysicallyDocumentDetailsDto
    {
        public List<OrderDocument> Documents { get; set; }
        public string Title { get; internal set; }
    }
    public class Action
    {
        public bool IsDefault { get; set; }
        public string Title { get; set; }
        public string ActionId { get; internal set; }
    }
    public class OrderDocument
    {
        public string DocumentId { get; internal set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public List<Action> Actions { get; set; }


    }
}
