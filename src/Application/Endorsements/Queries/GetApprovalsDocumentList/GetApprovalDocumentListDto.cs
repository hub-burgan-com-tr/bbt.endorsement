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
        public List<Action> HTMLActions { get; set; }
        public List<Action> PlainTextActions { get; set; }
        public List<Action> PDFActions { get; set; }


    }

    public class Action
    {
        public bool IsDefault { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }

    }
}
