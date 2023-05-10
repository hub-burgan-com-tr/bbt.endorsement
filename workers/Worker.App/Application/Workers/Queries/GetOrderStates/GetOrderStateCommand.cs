using Domain.Enums;
using MediatR;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;

namespace Worker.App.Application.Workers.Queries.GetOrderStates;

public class GetOrderStateCommand : IRequest<Response<GetOrderStateResponse>>
{
    public string OrderId { get; set; }
}

public class GetOrderStateCommandhandler : IRequestHandler<GetOrderStateCommand, Response<GetOrderStateResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTime _dateTime;

    public GetOrderStateCommandhandler(IApplicationDbContext context, IDateTime dateTime)
    {
        _context = context;
        _dateTime = dateTime;
    }


    public async Task<Response<GetOrderStateResponse>> Handle(GetOrderStateCommand request, CancellationToken cancellationToken)
    {
        var order = _context.Orders.FirstOrDefault(x => x.OrderId == request.OrderId);
        if (order == null)
            return Response<GetOrderStateResponse>.NotFoundException(request.OrderId, "Orders", 404);

        var orderState = (OrderState)Enum.Parse(typeof(OrderState), order.State.ToString());
        return Response<GetOrderStateResponse>.Success(new GetOrderStateResponse { OrderState = orderState }, 200);
    }
}
