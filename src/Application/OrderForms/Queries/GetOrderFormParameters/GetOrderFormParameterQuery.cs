using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.OrderForms.Queries.GetOrderFormParameters
{
    public class GetOrderFormParameterQuery : IRequest<Response<GetOrderFormParameterDto>>
    {
    }

    public class GetOrderFormParameterQueryHandler : IRequestHandler<GetOrderFormParameterQuery, Response<GetOrderFormParameterDto>>
    {
        private IApplicationDbContext _context;
        public GetOrderFormParameterQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<GetOrderFormParameterDto>> Handle(GetOrderFormParameterQuery request, CancellationToken cancellationToken)
        {
            var Process =  _context.Parameters.OrderBy(x=>x.Text).Where(x => x.ParameterTypeId == (int)ParameterType.Process).Select(x => new ParameterDto { Id = x.Id, Text = x.Text }).ToList();
            var States = _context.Parameters.OrderBy(x => x.Text).Where(x => x.ParameterTypeId == (int)ParameterType.State).Select(x => new ParameterDto { Id = x.Id, Text = x.Text }).ToList();
            var titles =  _context.OrderDefinitions.OrderBy(x => x.Title).Select(x => new OrderDefinionParameterDto { OrderDefinionId = x.OrderDefinitionId, Title = x.Title }).ToList();
            GetOrderFormParameterDto response = new GetOrderFormParameterDto
            {
                Process = Process,
                States = States,
                Titles = titles
            };
            return Response<GetOrderFormParameterDto>.Success(response, 200);
        }
    }

}
