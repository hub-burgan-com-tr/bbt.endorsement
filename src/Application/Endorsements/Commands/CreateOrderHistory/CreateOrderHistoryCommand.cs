using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Endorsements.Commands.CreateOrderHistory
{
    public class CreateOrderHistoryCommand : IRequest<Response<bool>>
    {
        public string OrderId { get; set; }
        public string DocumentId { get; set; }
        public string State { get; set; }
        public string Description { get; set; }

        public class OrderHistoryCommandHandler : IRequestHandler<CreateOrderHistoryCommand, Response<bool>>
        {
            private readonly IApplicationDbContext _context;

            public OrderHistoryCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Response<bool>> Handle(CreateOrderHistoryCommand request, CancellationToken cancellationToken)
            {
                _context.OrderHistories.Add(new Domain.Entities.OrderHistory
                {
                    OrderHistoryId=Guid.NewGuid().ToString(),
                    OrderId=request.OrderId,
                    DocumentId=request.DocumentId,
                    State = request.State,
                    Description = request.Description,
                    Created=DateTime.Now
                });
                 var result=  _context.SaveChanges();
                return Response<bool>.Success(result>0?true:false, 200);
            }
        }

    }
}
