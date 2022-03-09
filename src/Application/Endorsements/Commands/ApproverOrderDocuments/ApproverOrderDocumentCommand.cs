using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Endorsements.Commands.ApproverOrderDocuments
{
    public class ApproverOrderDocumentCommand : IRequest<Response<ApproverOrderDocumentResponse>>
    {
        public Guid OrderId { get; set; }

        public Guid DocumentId { get; set; }
    }

    public class ApproveOrderDocumentCommandHandler : IRequestHandler<ApproverOrderDocumentCommand, Response<ApproverOrderDocumentResponse>>
    {
        private IApplicationDbContext _context;

        public ApproveOrderDocumentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Response<ApproverOrderDocumentResponse>> Handle(ApproverOrderDocumentCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
