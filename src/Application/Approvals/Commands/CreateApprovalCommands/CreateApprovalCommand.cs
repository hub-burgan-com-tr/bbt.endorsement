using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Events.Approvals;
using MediatR;

namespace Application.Approvals.Commands.CreateApprovalCommands;

public class CreateApprovalCommand : IRequest<Response<bool>>
{
    public string InstanceId { get; set; }
    public string ApprovalTitle { get; set; }
}

public class CreateApprovalCommandHandler : IRequestHandler<CreateApprovalCommand, Response<bool>>
{
    private readonly IApplicationDbContext _context;

    public CreateApprovalCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Response<bool>> Handle(CreateApprovalCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Approval
        {
            ApprovalId = Guid.NewGuid().ToString(),
            InstanceId = request.InstanceId,
            ApprovalTitle = request.ApprovalTitle
        };

        entity.DomainEvents.Add(new ApprovalCreateEvent(entity));
        _context.Approvals.Add(entity);
        var i = _context.SaveChanges();

        return Response<bool>.Success(i > 0, 200);
    }
}

