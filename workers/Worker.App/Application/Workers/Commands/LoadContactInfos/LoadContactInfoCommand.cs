using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;
using Worker.App.Application.Internals.Models;

namespace Worker.App.Application.Workers.Commands.LoadContactInfos
{
    public class LoadContactInfoCommand : IRequest<Response<LoadContactInfoResponse>>
    {
        public string InstanceId { get; set; }
        public EmailSendType EmailSendType { get; set; }
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
            var citizenshipNumber = "";
            if (request.EmailSendType == EmailSendType.Customer)
            {
                var order = _context.Orders
                    .Include(x => x.Customer)
                    .Where(x => x.OrderId == request.InstanceId)
                    .FirstOrDefaultAsync().Result;

                if(order != null)
                {
                    if(order.Customer != null)
                    {
                        citizenshipNumber = order.Customer.CitizenshipNumber.ToString();
                    }
                }
            }
            else if (request.EmailSendType == EmailSendType.Person)
            {
                var order = _context.Orders
                    .Include(x => x.Person)
                    .Where(x => x.OrderId == request.InstanceId)
                    .FirstOrDefaultAsync().Result;

                if (order != null)
                {
                    if (order.Person != null)
                    {
                        citizenshipNumber = order.Person.CitizenshipNumber.ToString();
                    }
                }
            }

            //if (order == null)
            //    return Response<LoadContactInfoResponse>.NotFoundException("Order not found", 404);
            //if (order.Customer == null)
            //    return Response<LoadContactInfoResponse>.NotFoundException("Customer not found", 404);

            if (!string.IsNullOrEmpty(citizenshipNumber))
            {
                var persons = await _internalsService.GetCustomerSearchByName(new CustomerSearchRequest
                {
                    name = citizenshipNumber,
                    page = 1,
                    size = 10
                });
                var person = persons.Data.CustomerList.Where(x => x.RecordStatus == "A").FirstOrDefault();
                if (person != null)
                {
                    return Response<LoadContactInfoResponse>.Success(new LoadContactInfoResponse { Customer = person }, 200);
                }
            }

            return Response<LoadContactInfoResponse>.Fail(request.EmailSendType.ToString() + " not found", 404);

        }
    }
}
