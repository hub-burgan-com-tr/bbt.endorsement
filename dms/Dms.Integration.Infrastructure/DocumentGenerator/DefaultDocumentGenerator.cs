using Dms.Integration.Infrastructure.Enums;
using System.Text;

namespace Dms.Integration.Infrastructure.DocumentGenerator;
public interface IDocumentGenerator
{
    Task<byte[]> CreateDocument(DocumentDefinitionType key, byte[] content, DocumentOutputType doctype);
}

public class DefaultDocumentGenerator : IDocumentGenerator
{
    protected readonly IHtmlToPdfConverter htmlToPdfConverter;

    public DefaultDocumentGenerator(IHtmlToPdfConverter htmlToPdfConverter)
    {
        this.htmlToPdfConverter = htmlToPdfConverter;
    }

    public async Task<byte[]> CreateDocument(DocumentDefinitionType key, byte[] content, DocumentOutputType doctype)
    {
        if (doctype == DocumentOutputType.PDF)
        {
            return await htmlToPdfConverter.GeneratePdfContent(key, Encoding.UTF8.GetString(content));
        }
        return null;
    }
}

