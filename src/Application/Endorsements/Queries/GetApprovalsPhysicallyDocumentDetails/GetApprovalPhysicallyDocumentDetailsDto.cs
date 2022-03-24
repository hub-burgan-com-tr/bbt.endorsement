namespace Application.Endorsements.Queries.GetApprovalsPhysicallyDocumentDetails
{
    public class GetApprovalPhysicallyDocumentDetailsDto
    {
        public List<OrderDocument> Documents { get; set; }
        public string Title { get; internal set; }
    }
    public class DocumentAction
    {
        public bool IsDefault { get; set; }
        public string Title { get; set; }
        public string DocumentActionId { get; internal set; }
    }
    public class OrderDocument
    {
        public string DocumentId { get; internal set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public List<DocumentAction> DocumentActions { get; set; }


    }
}
