using Domain.Enums;
using MediatR;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;

namespace Worker.App.Application.Documents.Commands.UpdateDocumentStates
{
    public class UpdateDocumentActionCommand : IRequest<Response<UpdateDocumentActionResponse>>
    {
        public string OrderId { get; set; }
        public string DocumentId { get; set; }
        public string ActionId { get; set; }
    }

    public class UpdateDocumentActionCommandHandler : IRequestHandler<UpdateDocumentActionCommand, Response<UpdateDocumentActionResponse>>
    {
        private readonly IApplicationDbContext _context;

        public UpdateDocumentActionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Response<UpdateDocumentActionResponse>> Handle(UpdateDocumentActionCommand request, CancellationToken cancellationToken)
        {
            var document = _context.Documents.FirstOrDefault(x => x.OrderId == request.OrderId && x.DocumentId == request.DocumentId);
            if(document == null)
                return Task.FromResult(new Response<UpdateDocumentActionResponse>());

            var action = _context.DocumentActions.FirstOrDefault(x => x.DocumentId == document.DocumentId && x.DocumentActionId == request.ActionId);
            if(action == null)
                return Task.FromResult(new Response<UpdateDocumentActionResponse>());

            var actionIsSelected = _context.DocumentActions.FirstOrDefault(x => x.Document.OrderId == request.OrderId && x.DocumentId == document.DocumentId && x.DocumentActionId == action.DocumentActionId && x.IsSelected == true);
            if(actionIsSelected != null)
                return Task.FromResult(new Response<UpdateDocumentActionResponse>());

            action.IsSelected = true;
            if (action.Choice == (int)ActionType.Approve)
                document.State = ActionType.Approve.ToString();
            else if (action.Choice == (int)ActionType.Reject)
                document.State = ActionType.Reject.ToString();

            _context.Documents.Update(document);
            _context.DocumentActions.Update(action);
            _context.SaveChanges();

            return Task.FromResult(new Response<UpdateDocumentActionResponse>());
        }
    }
}
