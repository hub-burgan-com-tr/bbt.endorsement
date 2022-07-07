namespace Dms.Integration.Infrastructure.DocumentServices;

public class GetDocument
{
    public class Response
    {
        public byte[] Content { get; set; }
        public DocumentItem Document { get; set; }
    }
}