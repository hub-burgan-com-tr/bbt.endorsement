using Dms.Integration.Infrastructure.Extensions;
using Dms.Integration.Infrastructure.Attributes;
using System.Text;

namespace Dms.Integration.Infrastructure.Document;

public class DocumentContent
{
    [DoNotLog]
    public byte[] Content { get; set; }

    public string MimeType { get; set; }

    public string GetFileExtension()
    {
        return MimeType.ToFileExtension();
    }

    public DocumentContent SetHtmlContent(string content)
    {
        return SetHtmlContent(Encoding.UTF8.GetBytes(content));
    }

    public DocumentContent SetHtmlContent(byte[] content)
    {
        Content = content;
        MimeType = MimeTypeExtensions.HtmlMimeType;
        return this;
    }

    public DocumentContent SetPdfContent(string content)
    {
        return SetPdfContent(System.Text.Encoding.UTF8.GetBytes(content));
    }

    public DocumentContent SetPdfContent(byte[] content)
    {
        Content = content;
        MimeType = MimeTypeExtensions.PdfMimeType;
        return this;
    }

    public bool IsHtml
    {
        get
        {
            return MimeType.IsHtml();
        }
    }

    public bool IsPdf
    {
        get
        {
            return MimeType.IsPdf();
        }
    }
}
