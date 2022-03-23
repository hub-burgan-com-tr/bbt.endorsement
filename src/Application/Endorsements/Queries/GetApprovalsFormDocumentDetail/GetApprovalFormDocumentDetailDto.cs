namespace Application.Endorsements.Queries.GetApprovalsFormDocumentDetail
{
    public class GetApprovalFormDocumentDetailDto
    {
        /// <summary>
        /// Belge Link
        /// </summary>
        public string DocumentLink { get; set; }
        /// <summary>
        /// BelgeAd
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// TCKN
        /// </summary>
        public string CitizenShipNumber { get; set; }
        /// <summary>
        /// Ad Soyad
        /// </summary>
        public string FirstAndSurname { get; set; }
        /// <summary>
        /// Belge Onaylandımı
        /// </summary>
        public List<Action> Actions { get; set; }
        public string Title { get; internal set; }
        public string DocumentId { get; internal set; }
    }
    public class Action
    {
        public bool IsDefault { get; set; }
        public string Title { get; set; }
        public string ActionId { get; internal set; }
    }
}
