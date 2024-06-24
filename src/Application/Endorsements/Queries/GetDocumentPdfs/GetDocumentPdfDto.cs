namespace Application.Endorsements.Queries.GetDocumentPdfs;

public class GetDocumentPdfDto
{
    public string Path { get; set; }

    public Byte[] Bytes { get; set; }
    public string FileName { get; set; }
}

