namespace Dms.Integration.Infrastructure.DocumentServices;

public class IvrDocument
{
    public class Request
    {
        public string FileName { get; set; }
        public string DocType { get; set; }
        public string OwnerId { get; set; }
        public string MimeType { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string Channel { get; set; }
        public string CustomerNumber { get; set; }
        public byte[] Content { get; set; }
    }
    public class Response
    {
        public string DmsRefId { get; set; }
        public DateTime? ActionDate { get; set; }
        public string DocumentName { get; set; }
    }
}