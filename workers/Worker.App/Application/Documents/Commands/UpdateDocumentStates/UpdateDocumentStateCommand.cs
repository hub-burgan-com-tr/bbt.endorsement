using MediatR;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Coomon.Models;
using Worker.App.Domain.Enums;

namespace Worker.App.Application.Documents.Commands.UpdateDocumentStates
{
    public class UpdateDocumentStateCommand : IRequest<Response<UpdateDocumentStateResponse>>
    {
        public string OrderId { get; set; }
        public string DocumentId { get; set; }
        public string ActionId { get; set; }
        public bool IsSelected { get; set; }
    }

    public class UpdateDocumentStateCommandHandler : IRequestHandler<UpdateDocumentStateCommand, Response<UpdateDocumentStateResponse>>
    {
        private readonly IApplicationDbContext _context;

        public UpdateDocumentStateCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Response<UpdateDocumentStateResponse>> Handle(UpdateDocumentStateCommand request, CancellationToken cancellationToken)
        {
            var document = _context.Documents.FirstOrDefault(x => x.OrderId == request.OrderId && x.DocumentId == request.DocumentId);
            if(document == null)
                return Task.FromResult(new Response<UpdateDocumentStateResponse>());

            var action = _context.DocumentActions.FirstOrDefault(x => x.DocumentId == document.DocumentId && x.DocumentActionId == request.ActionId);
            if(action == null)
                return Task.FromResult(new Response<UpdateDocumentStateResponse>());

            if (request.IsSelected == true)
            {
                if (action.Choice == (int)ActionType.Approve)
                    document.State = ActionType.Approve.ToString();
                else if (action.Choice == (int)ActionType.Reject)
                    document.State = ActionType.Reject.ToString();
            }
            
            _context.Documents.Update(document);
            _context.SaveChanges();

            return Task.FromResult(new Response<UpdateDocumentStateResponse>());
        }
    }
}
