using Domain.Enums;
using MediatR;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;

namespace Worker.App.Application.Orders.Commands.UpdateOrderGroups;

public class UpdateOrderGroupCommand : IRequest<Response<bool>>
{
    public string OrderId { get; set; }
}

public class UpdateOrderGroupCommandHandler : IRequestHandler<UpdateOrderGroupCommand, Response<bool>>
{
    private IApplicationDbContext _context;
    private IDateTime _dateTime;

    public UpdateOrderGroupCommandHandler(IApplicationDbContext context, IDateTime dateTime)
    {
        _context = context;
        _dateTime = dateTime;
    }

    public Task<Response<bool>> Handle(UpdateOrderGroupCommand request, CancellationToken cancellationToken)
    {
        var orderGroup = _context.OrderGroups.FirstOrDefault(x => x.OrderMaps.Any(y => y.OrderId == request.OrderId));
        if(orderGroup != null)
        {
            var orders = _context.Orders
                .Where(x=> x.OrderMaps.Any(y => y.OrderGroupId == orderGroup.OrderGroupId && 
                                                y.Order.State == OrderState.Approve.ToString() &&
                                                y.Order.Documents.Any(z => z.FormDefinition != null)));

            var documents = _context.Documents.Where(x => x.Order.OrderMaps.Any(y => y.OrderGroupId == orderGroup.OrderGroupId));

        }
        throw new NotImplementedException();
    }
}
