using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.OrderForms.Queries.GetProcess
{
    public class GetProcessQuery : IRequest<Response<List<GetProcessDto>>>
    {
        public int ParameterTypeId { get; set; }
    }
    public class GetProcessQueryHandler : IRequestHandler<GetProcessQuery, Response<List<GetProcessDto>>>
    {
        private IApplicationDbContext _context;

        public GetProcessQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<GetProcessDto>>> Handle(GetProcessQuery request, CancellationToken cancellationToken)
        {
            var response = _context.Parameters.Where(x=>x.ParameterTypeId==request.ParameterTypeId).Select(x => new GetProcessDto { Id=x.Id,Text=x.Text }).OrderBy(x => x.Text).ToList();
            return Response<List<GetProcessDto>>.Success(response, 200);
        }
    }

}
