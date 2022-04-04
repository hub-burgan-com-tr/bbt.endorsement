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
        public int Choice { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string State { get;  set; }
    }
}
