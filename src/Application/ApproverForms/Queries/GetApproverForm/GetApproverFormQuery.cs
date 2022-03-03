using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Models;
using MediatR;

namespace Application.ApproverForms.Queries.GetApproverForm
{
    public class GetApproverFormQuery:IRequest<Response<GetApproverFormDto>>
    {
        public int ApprovalIdId { get; set; }
        public int FormId { get; set; }
    }

    public class GetApproverFormQueryQueryHandler : IRequestHandler<GetApproverFormQuery, Response<GetApproverFormDto>>
    {
        public async Task<Response<GetApproverFormDto>> Handle(GetApproverFormQuery request, CancellationToken cancellationToken)
        {
            var result = new GetApproverFormDto();
            return Response<GetApproverFormDto>.Success(result, 200);
        }
    }
}
