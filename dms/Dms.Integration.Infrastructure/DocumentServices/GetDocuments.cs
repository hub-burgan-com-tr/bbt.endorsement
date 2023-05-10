namespace Dms.Integration.Infrastructure.DocumentServices;

public class DocumentItem
{
    public string DocId { get; set; }
    public DocumentTag Group { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string FileExtension { get; set; }
    public string MimeType { get; set; }
    public DateTime DocDate { get; set; }
    public List<DocumentMetaData> MetaDataList { get; set; }
}
public class DocumentMetaData
{
    public string Key { get; set; }
    public string Value { get; set; }
}

public class GetDocuments
{
    public class Request
    {
        public string Channel { get; set; }
        public string CustomerId { get; set; }
        public string TagId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PageNum { get; set; }
        public int PageSize { get; set; }
    }
    public class Response
    {
        public List<DocumentItem> DocumentList { get; set; }
    }
}