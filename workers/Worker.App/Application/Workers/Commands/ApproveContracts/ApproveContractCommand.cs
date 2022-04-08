using MediatR;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Coomon.Models;

namespace Worker.App.Application.Workers.Commands.ApproveContracts
{
    public class ApproveContractCommand : IRequest<Response<ApproveContractResponse>>
    {
        public string OrderId { get; set; }
    }

    public class ApproveContractCommandHandler : IRequestHandler<ApproveContractCommand, Response<ApproveContractResponse>>
    {
        private readonly IApplicationDbContext _context;

        public ApproveContractCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Response<ApproveContractResponse>> Handle(ApproveContractCommand request, CancellationToken cancellationToken)
        {
            var documents = _context.Documents.Where(x => x.OrderId == request.OrderId);
            
            return Task.FromResult(new Response<ApproveContractResponse>());
        }
    }
}
