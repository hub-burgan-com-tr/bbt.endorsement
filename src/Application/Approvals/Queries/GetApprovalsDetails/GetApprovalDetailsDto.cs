namespace Application.Approvals.Queries.GetApprovalsDetails
{
    public class GetApprovalDetailsDto
    {
        public string ApprovalName { get; set; }
        public string ApprovalDescription { get; set; }
        public string DocumentLink { get; set; }
        public bool IsDocumentApproved { get; set; }

    }
}
