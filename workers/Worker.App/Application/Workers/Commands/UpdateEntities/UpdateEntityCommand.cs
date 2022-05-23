using MediatR;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;
using Domain.Enums;

namespace Worker.App.Application.Workers.Commands.UpdateEntities
{
    public class UpdateEntityCommand : IRequest<Response<UpdateEntityResponse>>
    {
        public string OrderId { get; set; }
    }

    public class UpdateEntityCommandHandler : IRequestHandler<UpdateEntityCommand, Response<UpdateEntityResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public UpdateEntityCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Response<UpdateEntityResponse>> Handle(UpdateEntityCommand request, CancellationToken cancellationToken)
        {
            var order = _context.Orders.FirstOrDefault(x => x.OrderId == request.OrderId);
            if (order == null)
                return Response<UpdateEntityResponse>.NotFoundException("Order bulunamadı", 404);

            if (order.State != OrderState.Pending.ToString())
            {
                var state = (OrderState)Enum.Parse(typeof(OrderState), order.State.ToString());
                return Response<UpdateEntityResponse>.Success(new UpdateEntityResponse { OrderState = state, IsUpdated = false }, 200);
            }

            order.State = OrderState.Timeout.ToString();
            order.LastModified = _dateTime.Now;
            order = _context.Orders.Update(order).Entity;
            _context.SaveChanges();

            var orderState = (OrderState)Enum.Parse(typeof(OrderState), order.State.ToString());
            return Response<UpdateEntityResponse>.Success(new UpdateEntityResponse { OrderState = orderState, IsUpdated = true }, 200);

        }
    }
}
