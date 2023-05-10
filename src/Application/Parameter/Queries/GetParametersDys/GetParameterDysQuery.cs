using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Parameter.Queries.GetParametersDys
{
    public class GetParameterDysQuery : IRequest<Response<List<GetParameterDysDto>>>
    {

    }
    public class GetParameterDysQueryHandler : IRequestHandler<GetParameterDysQuery, Response<List<GetParameterDysDto>>>
    {
        private IApplicationDbContext _context;

        public GetParameterDysQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<GetParameterDysDto>>> Handle(GetParameterDysQuery request, CancellationToken cancellationToken)
        {
            var response = _context.Parameters.Where(x => x.DmsReferenceId!=null).OrderByDescending(x=>x.Created).Select(x => new GetParameterDysDto {ParameterId=x.ParameterId, Text = x.Text, DmsReferenceId = x.DmsReferenceId, DmsReferenceKey = x.DmsReferenceKey, DmsReferenceName = x.DmsReferenceName }).ToList();
            return Response<List<GetParameterDysDto>>.Success(response, 200);
        }
    }

}
