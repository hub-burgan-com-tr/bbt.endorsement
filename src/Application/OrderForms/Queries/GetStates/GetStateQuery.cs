using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.OrderForms.Queries.GetStates
{
    public class GetStateQuery : IRequest<Response<List<GetStateDto>>>
    {
        public int ParameterTypeId { get; set; }
    }
    public class GetStateQueryHandler : IRequestHandler<GetStateQuery, Response<List<GetStateDto>>>
    {
        private IApplicationDbContext _context;

        public GetStateQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<GetStateDto>>> Handle(GetStateQuery request, CancellationToken cancellationToken)
        {
            var response = _context.Parameters.Where(x => x.ParameterTypeId ==request.ParameterTypeId).Select(x => new GetStateDto { Id = x.Id, Text = x.Text }).OrderBy(x => x.Text).ToList();
            return Response<List<GetStateDto>>.Success(response, 200);
        }
    }

}
