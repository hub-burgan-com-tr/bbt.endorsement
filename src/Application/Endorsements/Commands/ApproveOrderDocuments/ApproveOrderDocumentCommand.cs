using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Endorsements.Commands.ApproveOrderDocuments
{
    public class ApproveOrderDocumentCommand : IRequest<Response<ApproveOrderDocumentResponse>>
    {
        public Guid OrderId { get; set; }

        public Guid DocumentId { get; set; }
    }

    public class ApproveOrderDocumentCommandHandler : IRequestHandler<ApproveOrderDocumentCommand, Response<ApproveOrderDocumentResponse>>
    {
        private IApplicationDbContext _context;

        public ApproveOrderDocumentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Response<ApproveOrderDocumentResponse>> Handle(ApproveOrderDocumentCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
