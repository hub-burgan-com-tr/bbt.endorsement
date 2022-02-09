namespace Application.Approvals.Queries.GetMyApprovalsDetails
{
    public class GetMyApprovalDetailsDto
    {
        public string ApprovalName { get; set; }
        public bool IsDocumentApproved { get; set; }
        public string ApprovalIcon { get; set; }
        public string Approver { get; set; }
        public string ApproverDate { get; set; }
    }
}
