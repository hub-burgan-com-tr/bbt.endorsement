using Application.Common.Models;
using MediatR;

namespace Application.Approvals.Queries.CreateFormCommands
{
    public class CreateFormCommandQuery : IRequest<Response<bool>>
    {
        public int ApprovalId { get; set; }
        public string TC { get; set; }
        public string NameAndSurname { get; set; }
    }

    public  class CreateFormApprovalQueryHandler : IRequestHandler<CreateFormCommandQuery, Response<bool>>
    {
        public async Task<Response<bool>> Handle(CreateFormCommandQuery request, CancellationToken cancellationToken)
        {
            return Response<bool>.Success(true, 200);
        }
    }

}
