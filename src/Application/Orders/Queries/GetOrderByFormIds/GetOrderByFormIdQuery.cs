using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Enums;
using MediatR;

namespace Application.Orders.Queries.GetOrderByFormIds;

public class GetOrderByFormIdQuery : IRequest<Response<List<GetOrderByFormIdResponse>>>
{
    public string FormDefinitionId { get; set; }
    public long CitizenshipNumber { get; set; }
}

public class GetOrderByFormIdQueryHandler : IRequestHandler<GetOrderByFormIdQuery, Response<List<GetOrderByFormIdResponse>>>
{
    private IApplicationDbContext _context;
    private IDateTime _dateTime;

    public GetOrderByFormIdQueryHandler(IApplicationDbContext context, IDateTime dateTime)
    {
        _context = context;
        _dateTime = dateTime;
    }

    public async Task<Response<List<GetOrderByFormIdResponse>>> Handle(GetOrderByFormIdQuery request, CancellationToken cancellationToken)
    {
        var formDefinition = _context.FormDefinitions.FirstOrDefault(x => x.FormDefinitionId == request.FormDefinitionId);
        if (formDefinition != null && formDefinition.DependencyFormId != null && formDefinition.Source == "file")
        {
            var customer = _context.Customers.FirstOrDefault(x => x.CitizenshipNumber == request.CitizenshipNumber);
            var dependencyForm = _context.FormDefinitions.FirstOrDefault(x => x.FormDefinitionId == formDefinition.DependencyFormId);

            var orders = _context.Orders
                .Where(x => x.CustomerId == customer.CustomerId &&
                            x.Documents.Any(y => y.FormDefinitionId == dependencyForm.FormDefinitionId) &&
                            x.State == OrderState.Approve.ToString())
                .Select(x => new GetOrderByFormIdResponse
                {
                    OrderId = x.OrderId,
                    OrderName = x.Title
                })
                .ToList();

            return Response<List<GetOrderByFormIdResponse>>.Success(orders, 200);
        }
        return Response<List<GetOrderByFormIdResponse>>.Success(404);
    }

}
