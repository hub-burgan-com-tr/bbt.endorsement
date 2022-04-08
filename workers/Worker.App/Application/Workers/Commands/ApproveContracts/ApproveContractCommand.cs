using MediatR;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Coomon.Models;
using Worker.App.Domain.Enums;

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
            var order = _context.Orders.FirstOrDefault(x=> x.OrderId == request.OrderId && x.State == OrderState.Pending.ToString());

            if (order == null)
                return Response<ApproveContractResponse>.Fail("", 200);

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
                _context.Orders.Update(order);
                _context.SaveChanges();
            }

            var approve = documents.FirstOrDefault(x => x.State != ActionType.Approve.ToString());
            if (approve == null)
            {
                order.State = OrderState.Approve.ToString();
                order.LastModified = _dateTime.Now;
                _context.Orders.Update(order);
                _context.SaveChanges();
            }

            return new Response<ApproveContractResponse>();
        }
    }
}
