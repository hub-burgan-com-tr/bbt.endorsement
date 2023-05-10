using Microsoft.AspNetCore.Http;

namespace Application.Endorsements.Queries.GetDocumentPdfs;

public class GetDocumentPdfDto
{
    public string Path { get; set; }
    public IFormFile File { get; set; }

    public Byte[] Bytes { get; set; }
    public string FileName { get; set; }
}

