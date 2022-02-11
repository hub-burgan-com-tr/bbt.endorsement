using Application.Common.Models;
using MediatR;

namespace Application.OrderForms.Queries.GetForms
{
    public class GetFormQuery : IRequest<Response<List<GetFormDto>>>
    {
        public string InstanceId { get; set; }

    }

    public class GetFormApprovalQueryHandler : IRequestHandler<GetFormQuery, Response<List<GetFormDto>>>
    {
        public async Task<Response<List<GetFormDto>>> Handle(GetFormQuery request, CancellationToken cancellationToken)
        {
            var list = new List<GetFormDto>();
            return Response<List<GetFormDto>>.Success(list, 200);
        }
    }
}
