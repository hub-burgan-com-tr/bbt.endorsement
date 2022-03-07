namespace Application.Endorsements.Queries.GetApprovals
{
    public class GetApprovalDto 
    {
        /// <summary>
       /// Onay Id
      /// </summary>
        public int ApprovalId { get; set; }
        /// <summary>
        /// Onay Ad
        /// </summary>
        public string ApprovalName { get; set; }
        /// <summary>
        /// Belge Var Mı
        /// </summary>
        public bool IsDocument { get; set; }
    }
}
