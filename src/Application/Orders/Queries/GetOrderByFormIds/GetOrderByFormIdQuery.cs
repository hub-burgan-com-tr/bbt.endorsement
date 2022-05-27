using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
                // Eğer DependencyReuse false seçilmiş ise onaylayıcının daha önce onayladığı ve ilişkilendirilmişmiş kayıtlar seçilemeyecek.
                if (dependencyForm.DependecyReuse == false)
                {
                    var orderMaps = _context.OrderMaps.Include(x => x.Order.Reference)
                                                            .Where(x => x.Document.FormDefinitionId == dependencyForm.FormDefinitionId && 
                                                                   x.Order.CustomerId == customer.CustomerId &&
                                                                   x.Order.State == OrderState.Approve.ToString());

                    var orders = new List<GetOrderByFormIdResponse>();
                    foreach (var orderMap in orderMaps)
                    {
                        var order = _context.OrderMaps.Any(x => x.OrderGroupId == orderMap.OrderGroupId && 
                                                                x.Document.FormDefinitionId == formDefinition.FormDefinitionId && 
                                                                x.Order.State != OrderState.Cancel.ToString()); //  && x.Order.State != OrderState.Reject.ToString()
                        if(order == false)
                        {
                            orders.Add(new GetOrderByFormIdResponse
                            {
                                OrderId = orderMap.OrderId,
                                OrderName = orderMap.Order.Title + " - " + orderMap.Order.Reference.ProcessNo
                            });
                        }
                    }
                   
                    return Response<List<GetOrderByFormIdResponse>>.Success(orders, 200);
                }
                else
                {
                    var orders = _context.Orders
                          .Where(x => x.CustomerId == customer.CustomerId &&
                                      x.Documents.Any(y => y.FormDefinitionId == dependencyForm.FormDefinitionId) &&
                                      x.State == OrderState.Approve.ToString())
                          .Select(x => new GetOrderByFormIdResponse
                          {
                              OrderId = x.OrderId,
                              OrderName = x.Title + " - " + x.Reference.ProcessNo
                          })
                          .ToList();

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