using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Approvals.Commands.UpdateApprovalCommands;

public class UpdateApprovalCommand : IRequest<Response<bool>>
{
    public int Id { get; set; }
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

public class UpdateApprovalCommandHandler : IRequestHandler<UpdateApprovalCommand, Response<bool>>
{
    private readonly IApplicationDbContext _context;

    public UpdateApprovalCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Response<bool>> Handle(UpdateApprovalCommand request, CancellationToken cancellationToken)
    {
        return Response<bool>.Success(data:true, 200);
    }

}

