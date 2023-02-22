using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;

namespace Worker.App.Application.Workers.Queries.GetOrderConfigs
{
    public class GetOrderConfigCommand : IRequest<Response<GetOrderConfigResponse>>
    {
        public string OrderId { get; set; }
    }

    public class GetOrderConfigCommandHandler : IRequestHandler<GetOrderConfigCommand, Response<GetOrderConfigResponse>>
    {
        private IApplicationDbContext _context;
        private IDateTime _dateTime;

        public GetOrderConfigCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Response<GetOrderConfigResponse>> Handle(GetOrderConfigCommand request, CancellationToken cancellationToken)
        {
            var data = _context.Orders
                .Where(x => x.OrderId == request.OrderId.ToString())
                .Select(x => new
                {
                    x.Config.RetryFrequence,
                    x.Config.ExpireInMinutes,
                    x.Config.MaxRetryCount,
                    x.Config.IsPersonalMail,
                    x.Config.Device
                }).FirstOrDefault();

            var response = new GetOrderConfigResponse
            {
                MaxRetryCount = data.MaxRetryCount,
                ExpireInMinutes = "PT" + data.ExpireInMinutes + "M",
                RetryFrequence = "PT" + data.RetryFrequence + "M",
                IsPersonalMail = data.IsPersonalMail,
                Device = data.Device,
            };

            return Response<GetOrderConfigResponse>.Success(response, 200);
        }
    }
}
