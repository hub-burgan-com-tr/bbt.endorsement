using Domain.Entities;
using MediatR;
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
        _context.OrderHistories.Add(new OrderHistory
        {
            OrderHistoryId = Guid.NewGuid().ToString(),
            OrderId = request.OrderId,
            DocumentId = request.DocumentId,
            State = request.State,
            Description = request.Description,
            Created = _dateTime.Now,
            IsStaff = request.IsStaff
        });
        var result = _context.SaveChanges();
        return Response<bool>.Success(result > 0 ? true : false, 200);
    }
}