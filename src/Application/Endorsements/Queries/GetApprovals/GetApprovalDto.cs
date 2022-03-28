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
        public string Title { get; set; }
        /// <summary>
        /// Belge Var Mı
        /// </summary>
        public bool IsDocument { get; set; }
        public bool IsFormDetail{ get; set; }
    }
}
