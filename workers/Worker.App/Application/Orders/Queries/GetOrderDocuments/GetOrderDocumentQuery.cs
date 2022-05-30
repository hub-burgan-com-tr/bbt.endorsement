using MediatR;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;

namespace Worker.App.Application.Orders.Queries.GetOrderDocuments;

public class GetOrderDocumentQuery : IRequest<Response<List<GetOrderDocumentResponse>>>
{
    public string OrderId { get; set; }
}

public class GetOrderDocumentQueryHandler : IRequestHandler<GetOrderDocumentQuery, Response<List<GetOrderDocumentResponse>>>
{
    private IApplicationDbContext _context;

    public GetOrderDocumentQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Response<List<GetOrderDocumentResponse>>> Handle(GetOrderDocumentQuery request, CancellationToken cancellationToken)
    {
        var response = _context.Documents
            .Where(x => x.OrderId == request.OrderId)
            .Select(x=> new GetOrderDocumentResponse
            {
                DocumentId = x.DocumentId,
                Name = x.Name
            }).ToList();
        return Response<List<GetOrderDocumentResponse>>.Success(response, 200);
    }
}

