using Dms.Integration.Infrastructure.Extensions;
using Dms.Integration.Infrastructure.Attributes;

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
}
