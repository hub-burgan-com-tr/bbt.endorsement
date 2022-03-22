namespace Application.Endorsements.Queries.GetWatchApprovals
{
    public class GetWatchApprovalDto
    {
        /// <summary>
        /// Onay Id
        /// </summary>
        public string OrderId { get; set; }
        public string Title { get; set; }
        public string Approver { get; set; }
        public string Approval { get; set; }
        public string Process { get; set; }
        public string State { get; set; }
        public string ProcessNo { get; set; }
        public string TransactionNumber { get; set; }
        /// <summary>
        /// Onay Tarihi
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// Belge Var Mı
        /// </summary>
        public bool IsDocument { get; set; }

        public string OrderState { get; set; }

    }
}
