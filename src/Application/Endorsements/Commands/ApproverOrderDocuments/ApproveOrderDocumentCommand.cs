using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Endorsements.Commands.ApproveOrderDocuments
{
    public class ApproveOrderDocumentCommand : IRequest<Response<bool>>
    {
        public string OrderId { get; set; }

        public List<OrderDocument> Documents { get; set; }
    }
    public class OrderDocument
    {
        public string DocumentId { get; set; }
        public List<DocumentAction> Actions { get; set; }

    }
    public class DocumentAction
    {
        public string ActionId { get; set; }
        public bool IsDefault { get; set; }

    }
    public class ApproveOrderDocumentCommandHandler : IRequestHandler<ApproveOrderDocumentCommand, Response<bool>>
    {
        private IApplicationDbContext _context;

        public ApproveOrderDocumentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Response<bool>> Handle(ApproveOrderDocumentCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
