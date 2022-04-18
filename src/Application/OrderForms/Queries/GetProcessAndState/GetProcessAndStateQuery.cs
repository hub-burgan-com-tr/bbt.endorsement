using Application.Common.Interfaces;
using Application.Common.Models;
using Application.OrderForms.Queries.GetStates;
using MediatR;

namespace Application.OrderForms.Queries.GetProcessAndState
{

    public class GetProcessAndStateQuery : IRequest<Response<GetProcessAndStateDto>>
    {
        public int ProcessParameterTypeId { get; set; }
        public int StateParameterTypeId { get; set; }

    }

    public class GetProcessAndStateQueryHandler : IRequestHandler<GetProcessAndStateQuery, Response<GetProcessAndStateDto>>
    {
        private IApplicationDbContext _context;

        public GetProcessAndStateQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<GetProcessAndStateDto>> Handle(GetProcessAndStateQuery request, CancellationToken cancellationToken)
        {
            var response = new GetProcessAndStateDto { Process = _context.Parameters.Where(x => x.ParameterTypeId == request.ProcessParameterTypeId).OrderBy(x => x.Text).Select(x => new GetProcess.GetProcessDto { Id = x.Id, Text = x.Text }).ToList(), State = _context.Parameters.Where(x => x.ParameterTypeId == request.StateParameterTypeId).OrderBy(x => x.Text).Select(x => new GetStateDto { Id = x.Id, Text = x.Text }).ToList() };
            return Response<GetProcessAndStateDto>.Success(response, 200);
        }
    }

}
