using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Parameter.Queries.GetParameterTypes
{
    public class GetParameterTypeQuery : IRequest<Response<List<GetParameterTypeDto>>>
    {
    }

    public class GetParameterTypeQueryHandler : IRequestHandler<GetParameterTypeQuery, Response<List<GetParameterTypeDto>>>
    {
        private IApplicationDbContext _context;

        public GetParameterTypeQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<GetParameterTypeDto>>> Handle(GetParameterTypeQuery request, CancellationToken cancellationToken)
        {
            var response = _context.ParameterTypes.Select(x => new GetParameterTypeDto { ParameterTypeId=x.ParameterTypeId,Name=x.Name }).OrderBy(x => x.Name).ToList();
            return Response<List<GetParameterTypeDto>>.Success(response, 200);
        }
    }

}
