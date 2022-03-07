namespace Application.Endorsements.Queries.GetMyApprovals
{
    public class GetMyApprovalDto
    {/// <summary>
    /// Onay Id
    /// </summary>
        public int ApprovalId { get; set; }
    /// <summary>
    /// Onay Ad
    /// </summary>
        public string ApprovalName { get; set; }
    /// <summary>
    /// Belge Var Mi
    /// </summary>
        public bool IsDocument { get; set; }
    /// <summary>
    /// Belge Icon
    /// </summary>
        public string ApprovalIcon { get; set; }
    }   
}
