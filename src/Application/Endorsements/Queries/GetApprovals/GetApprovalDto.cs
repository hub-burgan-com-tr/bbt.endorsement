namespace Application.Endorsements.Queries.GetApprovals
{
    public class GetApprovalDto 
    {
        /// <summary>
       /// Onay Id
      /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// Onay Ad
        /// </summary>
        public string OrderName { get; set; }
        /// <summary>
        /// Belge Var Mı
        /// </summary>
        public bool IsDocument { get; set; }
    }
}
