namespace Worker.App.Application.Workers.Commands.SaveEntities
{
    public class SaveEntityResponse
    {
        public SaveEntityResponse()
        {
            Documents = new List<SaveEntityDocumentResponse>();
        }

        public string OrderId { get; set; }
        public int ExpireInMinutes { get; set; }
        public int RetryFrequence { get; set; }
        public int MaxRetryCount { get; set; }
        public List<SaveEntityDocumentResponse> Documents { get; set; }
    }

    public class SaveEntityDocumentResponse
    {
        public string DocumentId { get; set; }
        public string Name { get; set; }
    }
}
