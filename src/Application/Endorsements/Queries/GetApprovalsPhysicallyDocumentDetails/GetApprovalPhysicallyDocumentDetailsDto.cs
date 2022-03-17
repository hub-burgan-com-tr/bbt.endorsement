namespace Application.Endorsements.Queries.GetApprovalsPhysicallyDocumentDetails
{
    public class GetApprovalPhysicallyDocumentDetailsDto
    {        
        /// <summary>
        /// Belge Ad
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Belge Link
        /// </summary>
        public string DocumentLink { get; set; }

        /// <summary>
        /// Belge Onaylandı Mı
        /// </summary>
        public List<Action> Actions { get; set; }
    }
    public class Action
    {
        public bool IsDefault { get; set; }
        public string Title { get; set; }
    }
}
