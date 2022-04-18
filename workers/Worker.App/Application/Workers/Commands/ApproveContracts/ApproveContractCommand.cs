using Domain.Enums;
using MediatR;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;

namespace Worker.App.Application.Workers.Commands.ApproveContracts
{
    public class ApproveContractCommand : IRequest<Response<ApproveContractResponse>>
    {
        public string OrderId { get; set; }
    }

    public class ApproveContractCommandHandler : IRequestHandler<ApproveContractCommand, Response<ApproveContractResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public ApproveContractCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Response<ApproveContractResponse>> Handle(ApproveContractCommand request, CancellationToken cancellationToken)
        {
            var order = _context.Orders.FirstOrDefault(x=> x.OrderId == request.OrderId);

            if (order == null && order.State != OrderState.Pending.ToString())
            {
                var state = (OrderState)Enum.Parse(typeof(OrderState), order.State.ToString());
                return Response<ApproveContractResponse>.Success(new ApproveContractResponse { OrderState = state }, 200);
            }

            var documents = _context.Documents
                 .Where(x => x.OrderId == request.OrderId)
                 .Select(x => new GetOrderDocumentState
                 {
                     DocumentId = x.DocumentId,
                     State = x.State
                 }).AsEnumerable();

            var reject = documents.FirstOrDefault(x => x.State == ActionType.Reject.ToString());
            if(reject != null)
            {
                order.State = OrderState.Reject.ToString();
                order.LastModified = _dateTime.Now;
                order = _context.Orders.Update(order).Entity;
                _context.SaveChanges();
            }

            var approve = documents.FirstOrDefault(x => x.State != ActionType.Approve.ToString());
            if (approve == null)
            {
                order.State = OrderState.Approve.ToString();
                order.LastModified = _dateTime.Now;
                order = _context.Orders.Update(order).Entity;
                _context.SaveChanges();
            }

            var orderState = (OrderState)Enum.Parse(typeof(OrderState), order.State.ToString());

            var approveContractDocuments = _context.Documents
                .Where(x => x.OrderId == order.OrderId)
                .Select(x => new ApproveContractDocumentResponse
                {
                    DocumentId = x.DocumentId,
                    DocumentName = x.Name,
                    ActionTitle = x.DocumentActions.FirstOrDefault(x => x.IsSelected) != null ? x.DocumentActions.FirstOrDefault(x => x.IsSelected).Title : ""
                }).ToList();

            return Response<ApproveContractResponse>.Success(new ApproveContractResponse { OrderState = orderState, Documents = approveContractDocuments }, 200);
        }
    }
}
