using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Orders.Queries.GetOrderByFormIds;

public class GetOrderByFormIdQuery : IRequest<Response<List<GetOrderByFormIdResponse>>>
{
    public string FormDefinitionId { get; set; }
    public long CitizenshipNumber { get; set; }
    public OrderCustomer Approver { get; set; }
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
        if (formDefinition != null && !string.IsNullOrEmpty(formDefinition.DependencyFormId) && formDefinition.Source == "file")
        {
            var customer = _context.Customers.FirstOrDefault(x => x.CitizenshipNumber == request.CitizenshipNumber);
            if(customer == null)
            {
                var entity = _context.Customers.Add(new Customer
                {
                    CustomerId = Guid.NewGuid().ToString(),
                    CitizenshipNumber = request.Approver.CitizenshipNumber,
                    ClientNumber = request.Approver.ClientNumber,
                    FirstName = request.Approver.First,
                    LastName = request.Approver.Last,
                    Created = _dateTime.Now,
                }).Entity;

                _context.SaveChanges();
            }
            var dependencyForm = _context.FormDefinitions.FirstOrDefault(x => x.FormDefinitionId == formDefinition.DependencyFormId);

            if(dependencyForm != null)
            {
                if(dependencyForm.DependecyReuse == false)
                {
                    var orders = _context.Orders
                        .Where(x => x.OrderMaps.Any(y => y.OrderGroup.IsCompleted == false) &&
                                    x.CustomerId == customer.CustomerId &&
                                    x.Documents.Any(y => y.FormDefinitionId == dependencyForm.FormDefinitionId) &&
                                    x.State == OrderState.Approve.ToString())
                        .Select(x => new GetOrderByFormIdResponse
                        {
                            OrderId = x.OrderId,
                            OrderName = x.Title //+ " - " + x.Reference.ProcessNo
                        })
                        .ToList();

                    return Response<List<GetOrderByFormIdResponse>>.Success(orders, 200);
                }
                else
                {
                    var data = _context.OrderGroups
                        .Where(x => x.IsCompleted == false &&
                                    x.OrderMaps.Any(y => y.Order.CustomerId == customer.CustomerId &&
                                                         y.Order.Documents.Any(z => z.FormDefinitionId == dependencyForm.FormDefinitionId && z.Order.State == OrderState.Approve.ToString())) &&
                                    x.OrderMaps.Count() == 1)
                        .Select(x => new
                        {
                            Orders = x.OrderMaps.Select(y => new GetOrderByFormIdResponse
                            {
                                OrderId = y.OrderId,
                                OrderName = y.Order.Title + " - " + y.Order.Reference.ProcessNo
                            })
                        }).ToList();

                    var orders = data.FirstOrDefault().Orders.ToList();

                    return Response<List<GetOrderByFormIdResponse>>.Success(orders, 200);
                }
            }
        }
        return Response<List<GetOrderByFormIdResponse>>.Success(404);
    }
}

public class OrderCustomer
{
    public long CitizenshipNumber { get; set; }
    public string First { get; set; }
    public string Last { get; set; }
    public long ClientNumber { get; set; }
}