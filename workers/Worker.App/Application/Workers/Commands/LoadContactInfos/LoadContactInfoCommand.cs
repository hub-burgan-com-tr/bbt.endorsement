using MediatR;
using Microsoft.EntityFrameworkCore;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;

namespace Worker.App.Application.Workers.Commands.LoadContactInfos
{
    public class LoadContactInfoCommand : IRequest<Response<LoadContactInfoResponse>>
    {
        public string InstanceId { get; set; }
    }

    public class LoadContactInfoCommandHandler : IRequestHandler<LoadContactInfoCommand, Response<LoadContactInfoResponse>>
    {
        private IInternalsService _internalsService;
        private IApplicationDbContext _context;

        public LoadContactInfoCommandHandler(IInternalsService internalsService, IApplicationDbContext context)
        {
            _internalsService = internalsService;
            _context = context;
        }

        public async Task<Response<LoadContactInfoResponse>> Handle(LoadContactInfoCommand request, CancellationToken cancellationToken)
        {
            var order = _context.Orders
                .Include(x => x.Customer)
                .Where(x => x.OrderId == request.InstanceId)
                .FirstOrDefaultAsync().Result;
            if (order == null)
                return Response<LoadContactInfoResponse>.NotFoundException("Order not found", 404);

            var response = await _internalsService.GetPersonById(order.Customer.CitizenshipNumber);
            return Response<LoadContactInfoResponse>.Success(new LoadContactInfoResponse { Person = response.Data }, 200);
        }
    }
}
