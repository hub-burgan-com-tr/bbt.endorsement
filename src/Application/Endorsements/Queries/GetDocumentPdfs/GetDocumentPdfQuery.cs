using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Endorsements.Queries.GetDocumentPdfs;

public class GetDocumentPdfQuery : IRequest<Response<GetDocumentPdfDto>>
{
    public string OrderId { get; set; }
    public string DocumentId { get; set; }
}

public class GetDocumentPdfQueryHandler : IRequestHandler<GetDocumentPdfQuery, Response<GetDocumentPdfDto>>
{
    private readonly IApplicationDbContext _context;

    public GetDocumentPdfQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Response<GetDocumentPdfDto>> Handle(GetDocumentPdfQuery request, CancellationToken cancellationToken)
    {
        var document = _context.Documents.FirstOrDefault(x => x.OrderId == request.OrderId && x.DocumentId == request.DocumentId);
        if (document == null)
            return null;

        string fileName = document.DocumentId + ".pdf";
        var path = Path.Combine("files/", fileName);

        var response = new GetDocumentPdfDto { Path = path, FileName = fileName };

        //if (File.Exists(path))
        //{
        //    return Response<GetDocumentPdfDto>.Success(response, 200);
        //}

        var base64Content = document.Content.Split(',');
        var base64 = base64Content[1];

        Byte[] bytes = Convert.FromBase64String(base64);
        //  File.WriteAllBytes(path, bytes);

        response.Bytes = bytes;

        return Response<GetDocumentPdfDto>.Success(response, 200);

    }
}

