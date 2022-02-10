using Application.Common.Models;
using MediatR;

namespace Application.Approvals.Queries.GetFormCommands
{
    public class GetFormCommandQuery : IRequest<Response<List<GetFormCommandDto>>>
    {
        public string InstanceId { get; set; }

    }

    public class GetFormApprovalQueryHandler : IRequestHandler<GetFormCommandQuery, Response<List<GetFormCommandDto>>>
    {
        public async Task<Response<List<GetFormCommandDto>>> Handle(GetFormCommandQuery request, CancellationToken cancellationToken)
        {
            var list = new List<GetFormCommandDto>();
            return Response<List<GetFormCommandDto>>.Success(list, 200);
        }
    }
}
