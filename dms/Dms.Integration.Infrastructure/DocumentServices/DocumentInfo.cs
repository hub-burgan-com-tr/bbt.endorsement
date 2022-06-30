using Dms.Integration.Infrastructure.Document;
using Dms.Integration.Infrastructure.Extensions;
using Dms.Integration.Infrastructure.Models;
using System.Text;

namespace Dms.Integration.Infrastructure.DocumentServices;

public abstract class DocumentInfo
{
    public DocumentBase Document { get; set; }
    public Dictionary<string, string> DocumentParameters { get; set; }  //[RotaDMS].[dbo].[DocumentTag].TagId altinda bunlar
    public string OwnerName { get; set; }
    public string OwnerSurname { get; set; }
    public string ChannelReferenceId { get; set; }
    public string DmsPrefix { get; set; }

    protected DocumentInfo(DocumentBase document, string ownerName, string ownerSurName, string channelReferenceId = null)
    {
        Document = document;
        OwnerName = ownerName;
        OwnerSurname = ownerSurName;
        DocumentParameters = new Dictionary<string, string>();
        ChannelReferenceId = channelReferenceId;
        DmsPrefix = "DijitalBaşvuru";
    }

    public virtual DocumentContent Content
    {
        get
        {
            var content = Document.Contents.FirstOrDefault(x => x.MimeType == MimeTypeExtensions.PdfMimeType);
            if (content == null)
            {
                content = Document.Contents.FirstOrDefault(x => x.MimeType == MimeTypeExtensions.HtmlMimeType);
            }
            if (content == null)
            {
                content = Document.Contents.FirstOrDefault();
            }
            //Throw.If<CoreSystemException>(content == null, $"DYS'ye boş dosya kaydı yapılamaz ({Document.Definition.Key.GetAttributeDescription()} dokümanı).");
            return content;
        }
    }

    public virtual string FileName
    {
        get
        {
            return $"{OwnerName}{OwnerSurname}-{Document.Definition.Key}-{Guid.NewGuid()}." + Content.MimeType.Split("/")[1];
        }
    }

    public virtual string DocumentTypeDMSReferenceId
    {
        get
        {
            return Document.Definition.DmsReferenceId;
        }
    }

    public virtual string Description
    {
        get
        {
            if (Document.OwnerActionType.HasValue
                && Document.OwnerActionType.Value != DocumentActionType.None)
            {
                return $"{DmsPrefix} - {Document.Definition.Key} -  {Document.OwnerActionType.GetAttributeDescription()}";
            }
            return $"{DmsPrefix} - { Document.Definition.Key} ";
        }
    }

    public string ConstructDocumentTags()
    {
        var tags = new StringBuilder();

        foreach (var item in DocumentParameters)
        {
            tags = tags.AppendLine($"<{item.Key}>{item.Value}</{item.Key}>");
        }
        return $"{tags}";
    }
}

/// <summary>
/// BhsDocument
/// </summary>
public class BhsDocument : DocumentInfo
{
    public BhsDocument(DocumentBase document, DmsPerson owner, string channelReferenceId, int customerNo, string citizenshipNumber, string branchCode, string bhsOrderNo, string version)
        : base(document, owner.FirstName, owner.LastName, channelReferenceId)
    {
        //<M375><Field19SubeKodu>9620</Field19SubeKodu>
        //<Field01MusteriNo>151933</Field01MusteriNo>
        //<Field08TCKimlik>67174055724</Field08TCKimlik>
        //<Field09VergiNo>4930036169</Field09VergiNo></M375>
        DocumentParameters.Add("Field19SubeKodu", branchCode);
        DocumentParameters.Add("Field01MusteriNo", customerNo.ToString());
        DocumentParameters.Add("Field08TCKimlik", citizenshipNumber);
        DocumentParameters.Add("BHSVersion", version);
        DocumentParameters.Add("BHSOrder", bhsOrderNo);
    }
}