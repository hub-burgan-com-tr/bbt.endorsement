using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Approvals.Commands;

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
        _context.Approvals.Add(new Domain.Entities.Approval
        {
            ApprovalId = Guid.NewGuid().ToString(),
            InstanceId = request.InstanceId,
            ApprovalTitle = request.ApprovalTitle
        });

        _context.SaveChanges();

        return Response<bool>.Success(true, 200);
    }
}

