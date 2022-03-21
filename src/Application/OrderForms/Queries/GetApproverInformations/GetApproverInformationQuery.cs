using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Models;
using MediatR;

namespace Application.OrderForms.Queries.GetApproverInformations
{
    public class GetApproverInformationQuery:IRequest<Response<string>>
    {
        public int Type { get; set; }
        public string Value { get; set; }
    }

    public class GetFormApprovarQueryHandler : IRequestHandler<GetApproverInformationQuery, Response<string>>
    {
        public async Task<Response<string>> Handle(GetApproverInformationQuery request, CancellationToken cancellationToken)
        {
            return Response<string>.Success(data:"", 200);
        }
    }
}

