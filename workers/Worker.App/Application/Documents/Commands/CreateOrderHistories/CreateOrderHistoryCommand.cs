using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;

namespace Worker.AppApplication.Documents.Commands.CreateOrderHistories;

public class CreateOrderHistoryCommand : IRequest<Response<bool>>
{
    public string OrderId { get; set; }
    public string DocumentId { get; set; }
    public string State { get; set; }
    public string Description { get; set; }
    public bool IsStaff { get; set; } = false;
    public string Response { get; set; }
    public string Request { get; set; }
    public string CustomerId { get; set; }
    public string PersonId { get; set; }

}

public class CreateOrderHistoryCommandHandler : IRequestHandler<CreateOrderHistoryCommand, Response<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTime _dateTime;

    public CreateOrderHistoryCommandHandler(IApplicationDbContext context, IDateTime dateTime)
    {
        _context = context;
        _dateTime = dateTime;
    }

    public async Task<Response<bool>> Handle(CreateOrderHistoryCommand request, CancellationToken cancellationToken)
    {
        var order =  await _context.Orders.FirstOrDefaultAsync(x => x.OrderId == request.OrderId);
        _context.OrderHistories.Add(new OrderHistory
        {
            OrderHistoryId = Guid.NewGuid().ToString(),
            OrderId = request.OrderId,
            DocumentId = request.DocumentId,
            State = request.State,
            Description = request.Description,
            Created = _dateTime.Now,
            IsStaff = request.IsStaff,
            Request = request.Request,
            Response = request.Response,
            PersonId = order?.PersonId,
            CustomerId = request.CustomerId,
        });
         await _context.SaveChangesAsync(cancellationToken);
        return Response<bool>.Success(true,200);
    }
}