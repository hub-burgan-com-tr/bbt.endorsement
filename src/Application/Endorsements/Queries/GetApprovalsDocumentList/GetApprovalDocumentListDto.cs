namespace Application.Endorsements.Queries.GetApprovalsDocumentList
{
    public class GetApprovalDocumentListDto
    {
        /// <summary>
        /// Belge Ad
        /// </summary>
        /// <summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Belge Onaylandımı
        /// </summary>
        public List<Action> Actions { get; set; }
    


    }

    public class Action
    {
        public string DocumentId { get; set; }
        public string Title { get; set; }
        public bool Checked { get; set; }
    }
}
