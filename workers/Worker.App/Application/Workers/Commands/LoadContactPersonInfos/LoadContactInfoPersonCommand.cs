using MediatR;
using Microsoft.EntityFrameworkCore;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;
using Worker.App.Application.Internals.Models;

namespace Worker.App.Application.Workers.Commands.LoadContactInfos
{
    public class LoadContactInfoPersonCommand : IRequest<Response<LoadContactInfoPersonResponse>>
    {
        public string InstanceId { get; set; }
    }

    public class LoadContactInfoPersonCommandHandler : IRequestHandler<LoadContactInfoPersonCommand, Response<LoadContactInfoPersonResponse>>
    {
        private IInternalsService _internalsService;
        private IApplicationDbContext _context;

        public LoadContactInfoPersonCommandHandler(IInternalsService internalsService, IApplicationDbContext context)
        {
            _internalsService = internalsService;
            _context = context;
        }

        public async Task<Response<LoadContactInfoPersonResponse>> Handle(LoadContactInfoPersonCommand request, CancellationToken cancellationToken)
        {
            var order = _context.Orders
                .Include(x => x.Person)
                .Where(x => x.OrderId == request.InstanceId)
                .FirstOrDefault();

            if (order == null)
                return Response<LoadContactInfoPersonResponse>.NotFoundException("Order not found", 404);
            if (order.Person == null)
                return Response<LoadContactInfoPersonResponse>.NotFoundException("Person not found", 404);

            var person = await _internalsService.GetCustomerSearchByName(new CustomerSearchRequest
            {
                name = order.Person.CitizenshipNumber.ToString(),
                page = 1,
                size = 10
            });

            return Response<LoadContactInfoPersonResponse>.Success(new LoadContactInfoPersonResponse { Customer = person.Data.CustomerList.FirstOrDefault() }, 200);
        }
    }
}
