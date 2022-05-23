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

    public async Task<Response<bool>> Handle(UpdateOrderGroupCommand request, CancellationToken cancellationToken)
    {
        var orderGroup = _context.OrderGroups.FirstOrDefault(x => x.OrderMaps.Any(y => y.OrderId == request.OrderId && y.Order.State == OrderState.Approve.ToString()));
        if(orderGroup != null)
        {
            var dependencyOrderMap = _context.OrderMaps
                .Where(x => x.DocumentId != null && 
                            x.OrderGroupId == orderGroup.OrderGroupId &&
                            x.Document.Order.State == OrderState.Approve.ToString() &&
                            x.Document.FormDefinition.DependencyFormId != null)
                .Select(x => new
                {
                    x.Document.FormDefinitionId
                }).FirstOrDefault();

            if(dependencyOrderMap != null)
            {
                var formDefinition = _context.FormDefinitions.FirstOrDefault(x=> x.DependencyFormId == dependencyOrderMap.FormDefinitionId);
                if(formDefinition != null)
                {
                    var formOrderMap = _context.OrderMaps
                        .Where(x => x.DocumentId != null &&
                                    x.OrderGroupId == orderGroup.OrderGroupId &&
                                    x.Document.Order.State == OrderState.Approve.ToString() &&
                                    x.Document.FormDefinition.FormDefinitionId == formDefinition.FormDefinitionId)
                        .FirstOrDefault();

                    //var document = _context.Documents
                    //    .Where(x => x.Order.OrderMaps.Any(y => y.OrderGroupId == orderGroup.OrderGroupId) &&
                    //                x.FormDefinition.FormDefinitionId == formDefinition.FormDefinitionId &&
                    //                x.Order.State == OrderState.Approve.ToString());

                    if(formOrderMap != null)
                    {
                        orderGroup.IsCompleted = true;
                        _context.OrderGroups.Update(orderGroup);
                        _context.SaveChanges();
                        return Response<bool>.Success(200);
                    }
                }
            }
        }
        return Response<bool>.NotFoundException("Kayıt bulunamadı", 404);
    }
}
