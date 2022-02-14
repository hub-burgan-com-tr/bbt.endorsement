using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Events.Approvals;
using MediatR;

namespace Application.Approvals.Commands.CreateApprovalCommands;

public class CreateApprovalCommand : IRequest<Response<bool>>
{
    /// <summary>
    /// InstanceId
    /// </summary>
    public string InstanceId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Title { get; set; }
    public string Process { get; set; }
    public string Stage { get; set; }
    public string TransactionNumber { get; set; }
    public string TimeoutMinutes { get; set; }
    public string RetryFrequence { get; set; }
    public int MaxRetryCount { get; set; }
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
            ApprovalTitle = request.Title
        };

        entity.DomainEvents.Add(new ApprovalCreateEvent(entity));
        _context.Approvals.Add(entity);
        var i = _context.SaveChanges();

        return Response<bool>.Success(i > 0, 200);
    }
}

