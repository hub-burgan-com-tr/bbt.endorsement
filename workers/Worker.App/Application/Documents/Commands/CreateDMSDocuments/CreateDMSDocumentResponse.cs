namespace Worker.App.Application.Documents.Commands.CreateDMSDocuments;

public class CreateDMSDocumentResponse
{
    public string DmsRefId { get; set; } // CreateDMSDocument Gelen
    public int? DmsReferenceKey { get; set; }
    public string DmsReferenceName { get; set; }
}