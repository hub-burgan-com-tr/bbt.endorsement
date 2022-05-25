using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.OrderForms.Queries.GetOrderFormTags
{
    public class GetOrderFormTagQuery : IRequest<Response<List<GetOrderFormTagDto>>>
    {

    }
    public class GetOrderFormTagQueryHandler : IRequestHandler<GetOrderFormTagQuery, Response<List<GetOrderFormTagDto>>>
    {
        private IApplicationDbContext _context;

        public GetOrderFormTagQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<GetOrderFormTagDto>>> Handle(GetOrderFormTagQuery request, CancellationToken cancellationToken)
        {
            var response = _context.FormDefinitionActions.Where(x=>x.FormDefinitionId== "fd95116e-e7e0-4cdf-b734-11c414c3a471").Select(x => new GetOrderFormTagDto { OrderDefinitionActionId = x.FormDefinitionActionId, Title = x.Title }).ToList();
            return Response<List<GetOrderFormTagDto>>.Success(response, 200);
        }
    }

}
