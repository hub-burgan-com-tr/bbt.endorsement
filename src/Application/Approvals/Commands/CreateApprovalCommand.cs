using Application.Common.Models;
using MediatR;

namespace Application.Approvals.Commands;

public class CreateApprovalCommand : IRequest<Response<bool>>
{
    public string? InstanceId { get; set; }
}

public class CreateApprovalCommandHandler : IRequestHandler<CreateApprovalCommand, Response<bool>>
{
    public async Task<Response<bool>> Handle(CreateApprovalCommand request, CancellationToken cancellationToken)
    {
        return Response<bool>.Success(true, 200);
    }
}

