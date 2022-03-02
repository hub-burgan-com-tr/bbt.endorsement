using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Endorsements.Queries.ApproveOrderDocuments
{
    public class ApproveOrderDocumentQuery : IRequest<Response<ApproveOrderDocumentResponse>>
    {
    }

    public class ApproveOrderDocumentQueryHandler : IRequestHandler<ApproveOrderDocumentQuery, Response<ApproveOrderDocumentResponse>>
    {
        private IApplicationDbContext _context;

        public ApproveOrderDocumentQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Response<ApproveOrderDocumentResponse>> Handle(ApproveOrderDocumentQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
