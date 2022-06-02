using Dms.Integration.Infrastructure.Document;
using Dms.Integration.Infrastructure.DocumentServices;
using Dms.Integration.Infrastructure.Extensions;
using DmsIntegrationService;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Dms.Integration.Infrastructure.Services;

public interface IDocumentService
{
    string CreateDMSDocument(DocumentInfo document);
    void UpdateDMSDocument(string docDmsId, DocumentInfo document);
    DocumentContent FetchDocumentFromDMS(string documentId);
    Task<DocumentGroups> GetDocumentGroupsAsync(string customerId, string channel);
    Task<GetDocuments.Response> GetDocumentsAsync(GetDocuments.Request request);
    Task<GetDocument.Response> GetDocumentContentAsync(string customerId, string documentId);
    Task<IvrDocument.Response> CreateIVRDocument(IvrDocument.Request request);
}

public class DocumentService : IDocumentService
{
    private readonly IPdfConverterService _pdfConverterService;
    private readonly ServiceCaller _caller;
    private readonly string _url;

    public DocumentService(IConfigurationRoot config, ServiceCaller caller, IPdfConverterService pdfConverterService)
    {
        _caller = caller;
        _pdfConverterService = pdfConverterService;
        _url = config.GetDMSServiceUrl();
    }

    public string CreateDMSDocument(DocumentInfo document)
    {
        if (string.IsNullOrEmpty(_url))
            return _url;
        StringBuilder builder = new StringBuilder();
        builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        builder.Append($"<document>");
        builder.Append($"<fileName>{document.FileName}</fileName>");
        builder.Append($"<mimeType>{document.Content.MimeType}</mimeType>");
        builder.Append($"<ownerID>908</ownerID>");
        builder.Append($"<desc>{document.Description}</desc>");
        builder.Append($"<notes>{document.FileName}</notes>");
        builder.Append($"<channel>{document.ChannelReferenceId}</channel>");
        builder.Append("<tagInfo>");
        builder.Append("<tagInfo>");
        builder.Append("<tagList>");
        builder.Append("<tag type=\"tag\">" + document.DocumentTypeDMSReferenceId + "</tag>");
        builder.Append("</tagList>");
        builder.Append("<tagData>");
        builder.Append("<tag type=\"data\" id=\"" + document.DocumentTypeDMSReferenceId + "\">");
        builder.Append($"<M{document.DocumentTypeDMSReferenceId}>");
        builder.Append(document.ConstructDocumentTags());
        builder.Append($"</M{document.DocumentTypeDMSReferenceId}>");
        builder.Append("</tag>");
        builder.Append("</tagData>");
        builder.Append("</tagInfo>");
        builder.Append("</tagInfo>");
        builder.Append($"</document>");

        var request = new AddDocumentRequest()
        {
            binaryData = document.Content.Content,
            cmdData = builder.ToString()
        };
        var result = _caller.Call<DmsServiceSoap, string>(_url, (proxy) =>
        {
            var response = proxy.AddDocumentAsync(request).Result;
            return response.AddDocumentResult;
        }, false, new { binaryData_Len = document.Content?.Content?.Length, request.cmdData });
        return result;
    }

    public DocumentContent FetchDocumentFromDMS(string documentId)
    {
        var result = _caller.Call<DmsServiceSoap, DocumentResponse>(_url, (proxy) =>
        {
            var response = proxy.GetDocumentAsync(documentId).Result;
            return response;
        });
        var document = new DocumentContent()
        {
            MimeType = result.FInfo.MimeType,
            Content = result.BinaryData
        };
        return document;
    }

    public async Task<DocumentGroups> GetDocumentGroupsAsync(string customerId, string channel)
    {
        var result = await _caller.CallAsync<DmsServiceSoap, IbGetDocumentGroupsResponse>(_url, (proxy) =>
        {
            var response = proxy.IbGetDocumentGroupsAsync(new IbGetDocumentGroupsRequest
            {
                Channel = channel,
                CustomerId = customerId
            });
            return response;
        });
        var response = new DocumentGroups();
        var documentTagList = result.TagGroupList?.Select(q => new DocumentTag
        {
            TagId = q.TagId,
            TagName = q.TagName
        }).ToList();
        response.TagList = documentTagList ?? new List<DocumentTag>();
        return response;
    }

    public async Task<GetDocuments.Response> GetDocumentsAsync(GetDocuments.Request request)
    {
        var result = await _caller.CallAsync<DmsServiceSoap, IbGetDocumentsResponse>(_url, (proxy) =>
        {
            var response = proxy.IbGetDocumentsAsync(new IbGetDocumentsRequest
            {
                Channel = request.Channel,
                CustomerId = request.CustomerId,
                EndDate = request.EndDate,
                PageNum = request.PageNum,
                PageSize = request.PageSize,
                StartDate = request.StartDate,
                TagId = request.TagId
            });
            return response;
        });

        var documentList = result.IbDocumentList?.Select(q => new DocumentItem
        {
            Description = q.Description,
            DocDate = q.DocDate,
            DocId = q.DocId,
            FileExtension = canConvertToPdf(q.MimeType) ? ".pdf" : q.FileExtension,
            Group = new DocumentTag
            {
                TagId = q.Group?.TagId,
                TagName = q.Group?.TagName
            },
            MimeType = canConvertToPdf(q.MimeType) ? "application/pdf" : q.MimeType,
            Name = q.Name,
            MetaDataList = q.MetaDataList?.Select(y => new DocumentMetaData
            {
                Key = y.Key,
                Value = y.Value
            }).ToList(),
        }).ToList();
        var response = new GetDocuments.Response
        {
            DocumentList = documentList ?? new System.Collections.Generic.List<DocumentItem>(),
        };
        return response;
    }

  

    public void UpdateDMSDocument(string docDmsId, DocumentInfo document)
    {
        //Throw.If<ArgumentNullException>(document == null, $"Document is null");
        //Throw.If<ArgumentNullException>(string.IsNullOrWhiteSpace(document.OwnerName)
                                        //|| string.IsNullOrWhiteSpace(document.OwnerSurname), $"Document.Owner is null");

        StringBuilder builder = new StringBuilder();
        builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        builder.Append("<tagInfo>");
        builder.Append("<tagList>");
        builder.Append("<tag type=\"tag\">" + document.DocumentTypeDMSReferenceId + "</tag>");
        builder.Append("</tagList>");
        builder.Append("<tagData>");
        builder.Append("<tag type=\"data\" id=\"" + document.DocumentTypeDMSReferenceId + "\">");
        builder.Append($"<M{document.DocumentTypeDMSReferenceId}>");
        builder.Append(document.ConstructDocumentTags());
        builder.Append($"</M{document.DocumentTypeDMSReferenceId}>");
        builder.Append("</tag>");
        builder.Append("</tagData>");
        builder.Append("</tagInfo>");

        var metadata = builder.ToString();
        _caller.Call<DmsServiceSoap, bool>(_url, (proxy) =>
        {
            var response = proxy.UpdateDocumentAsync(docDmsId, metadata).Result;
          //  Throw.If<CoreSystemException>(!response, $"DMS document {docDmsId} could not be updated!");
            return response;
        }, false, new { docId = docDmsId, cmdData = metadata });
    }

    private bool canConvertToPdf(string mimeType)
    {
        if (mimeType.Contains("pdf"))
        {
            return false;
        }
        if (isImageFormat(mimeType))
        {
            return true;
        }
        if (isHtmlFormat(mimeType))
        {
            return true;
        }
        return false;
    }
    private bool isImageFormat(string mimeType)
    {
        if (mimeType.Contains("jpeg")
            || mimeType.Contains("jpg")
            || mimeType.Contains("png")
            || mimeType.Contains("tif")
            || mimeType.Contains("gif")
            || mimeType.Contains("bmp"))
        {
            return true;
        }
        return false;
    }
    private bool isHtmlFormat(string mimeType)
    {
        if (mimeType.Contains("text/html"))
        {
            return true;
        }
        return false;
    }

    public async Task<GetDocument.Response> GetDocumentContentAsync(string customerId, string documentId)
    {
        var result = await _caller.CallAsync<DmsServiceSoap, IbGetDocumentContentResponse>(_url, (proxy) =>
        {
            var response = proxy.IbGetDocumentContentAsync(new IbGetDocumentContentRequest
            {
                CustomerId = customerId,
                DocumentId = documentId
            });
            return response;
        });
        if (result?.Content == null)
        {
            return new GetDocument.Response();
        }
        var response = new GetDocument.Response
        {
            Content = result.Content,
            Document = new DocumentItem
            {
                Description = result.IbDocument?.Description,
                DocDate = result.IbDocument.DocDate,
                DocId = result.IbDocument?.DocId,
                FileExtension = result.IbDocument?.FileExtension,
                Group = new DocumentTag
                {
                    TagId = result.IbDocument?.Group?.TagId,
                    TagName = result.IbDocument?.Group?.TagName
                },
                MimeType = result.IbDocument?.MimeType,
                Name = result.IbDocument?.Name,
                MetaDataList = result.IbDocument?.MetaDataList?.Select(y => new DocumentMetaData
                {
                    Key = y.Key,
                    Value = y.Value
                }).ToList(),
            }
        };
        if (canConvertToPdf(response.Document.MimeType))
        {

            if (isHtmlFormat(response.Document.MimeType))
            {
                var htmlString = System.Text.Encoding.UTF8.GetString(response.Content);
                response.Content = await this._pdfConverterService.GeneratePdfContent(htmlString);
            }
            else if (isImageFormat(response.Document.MimeType))
            {
                response.Content = this._pdfConverterService.ImagesToPdfContent(new List<byte[]> { response.Content });
            }
            response.Document.MimeType = "application/pdf";
            response.Document.FileExtension = ".pdf";
        }
        return response;
    }

    public async Task<IvrDocument.Response> CreateIVRDocument(IvrDocument.Request request)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        builder.Append($"<document>");
        builder.Append($"<fileName>{request.FileName}</fileName>");
        builder.Append($"<mimeType>{request.MimeType}</mimeType>");
        builder.Append($"<ownerID>{request.OwnerId}</ownerID>");
        builder.Append($"<desc>{request.Description}</desc>");
        builder.Append($"<notes>{request.Notes}</notes>");
        builder.Append($"<channel>{request.Channel}</channel>");

        builder.Append($"<wfInstanceID></wfInstanceID>");
        builder.Append($"<tagInfo>");
        builder.Append($"<tagInfo>");
        builder.Append($"<tagList>");
        builder.Append($"<tag type=\"tag\">{request.DocType}</tag>");
        builder.Append($"</tagList>");
        builder.Append($"<tagData>");
        builder.Append($"<tag type=\"data\" id=\"" + request.DocType + "\">");
        builder.Append($"<M{request.DocType}>");
        builder.Append($"<Field01MusteriNo>{ request.CustomerNumber}</Field01MusteriNo>");
        builder.Append($"</M{request.DocType}>");
        builder.Append($"</tag>");
        builder.Append($"</tagData>");
        builder.Append($"</tagInfo>");
        builder.Append($"</tagInfo>");
        builder.Append($"</document>");

        var documentRequest = new AddDocumentRequest()
        {
            binaryData = request.Content,
            cmdData = builder.ToString()
        };
        var result = await _caller.CallAsync<DmsServiceSoap, AddDocumentResponse>(_url, (proxy) =>
        {
            return proxy.AddDocumentAsync(documentRequest);
        }, false, new { binaryData_Len = request.Content?.Length, documentRequest.cmdData });
        return new IvrDocument.Response { DmsRefId = result.AddDocumentResult };
    }
}

