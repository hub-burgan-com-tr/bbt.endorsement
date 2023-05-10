using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Endorsements.Queries.GetOrderDocuments
{
    public class GetOrderDocumentQuery : IRequest<Response<OrderDocumentResponse>>
    {
        public Guid OrderId { get; set; }
    }

    public class GetOrderDocumentQueryHandler : IRequestHandler<GetOrderDocumentQuery, Response<OrderDocumentResponse>>
    {
        private IApplicationDbContext _context;

        public GetOrderDocumentQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Response<OrderDocumentResponse>> Handle(GetOrderDocumentQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
