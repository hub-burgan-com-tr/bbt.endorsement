using MediatR;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;
using Worker.App.Domain.Enums;

namespace Worker.App.Application.Workers.Commands.DeleteEntities
{
    public class DeleteEntityCommand : IRequest<Response<DeleteEntityResponse>>
    {
        public string OrderId { get; set; }
    }

    public class DeleteEntityCommandHandler : IRequestHandler<DeleteEntityCommand, Response<DeleteEntityResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public DeleteEntityCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Response<DeleteEntityResponse>> Handle(DeleteEntityCommand request, CancellationToken cancellationToken)
        {
            var order = _context.Orders.FirstOrDefault(x => x.OrderId == request.OrderId);

            if (order == null && order.State != OrderState.Pending.ToString())
            {
                var state = (OrderState)Enum.Parse(typeof(OrderState), order.State.ToString());
                return Response<DeleteEntityResponse>.Success(new DeleteEntityResponse { OrderState = state, IsUpdated = false }, 200);
            }

            order.State = OrderState.Cancel.ToString();
            order.LastModified = _dateTime.Now;
            order = _context.Orders.Update(order).Entity;
            _context.SaveChanges();

            var orderState = (OrderState)Enum.Parse(typeof(OrderState), order.State.ToString());
            return Response<DeleteEntityResponse>.Success(new DeleteEntityResponse { OrderState = orderState, IsUpdated = true }, 200);
        }
    }
}
