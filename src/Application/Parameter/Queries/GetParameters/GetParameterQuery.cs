using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Parameter.Queries.GetParameters
{
    public class GetParameterQuery : IRequest<Response<List<GetParameterDto>>>
    {
        public string Text { get; set; }
    }
    public class GetParameterQueryHandler : IRequestHandler<GetParameterQuery, Response<List<GetParameterDto>>>
    {
        private IApplicationDbContext _context;

        public GetParameterQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<GetParameterDto>>> Handle(GetParameterQuery request, CancellationToken cancellationToken)
        {
            var response = _context.Parameters.Where(x=>x.Text==request.Text).Select(x => new GetParameterDto {ParameterId=x.ParameterId ,Text = x.Text, DmsReferenceId = x.DmsReferenceId,DmsReferenceKey=x.DmsReferenceKey,DmsReferenceName=x.DmsReferenceName }).OrderBy(x => x.Text).ToList();
            return Response<List<GetParameterDto>>.Success(response, 200);
        }
    }

}
