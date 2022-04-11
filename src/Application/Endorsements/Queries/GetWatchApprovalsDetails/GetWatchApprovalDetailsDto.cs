namespace Application.Endorsements.Queries.GetWatchApprovalsDetails
{
    public class GetWatchApprovalDetailsDto
    {
        public string OrderId { get; set; }

        /// <summary>
        ///Baslik
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// İşlem
        /// </summary>
        public string NameAndSurname { get; set; }
        public string Process { get; set; }
        public string State { get; set; }
        public string ProcessNo { get; set; }
        /// <summary>
        /// Geçerlilik
        /// </summary>
        public int MaxRetryCount { get; set; }
        public int RetryFrequence { get; set; }
        public int ExpireInMinutes { get; set; }

        /// <summary>
        /// Belgeler
        /// </summary>
        public List<GetWatchApprovalDocumentDetailsDto> Documents { get; set; }
        /// <summary>
        /// Tarihce
        /// </summary>
        public List<GetWatchApprovalDetailsHistoryDto> History { get; set; }
        public string OrderState { get; internal set; }
    }


    public class GetWatchApprovalDocumentDetailsDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public string DocumentId { get; set; }
        /// <summary>
        /// Belge Tipi
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// Belge Adı
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Belge Onay Ad
        /// </summary>
        public string Title { get; set; }
        public string Content { get; internal set; }
        public string Type { get; internal set; }
    }
    public class GetWatchApprovalDetailsHistoryDto
    {
        public string State { get; set; }
        /// <summary>
        /// Belge
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// İşlem Tarihi
        /// </summary>
        public string CreatedDate { get; set; }

    }
}
