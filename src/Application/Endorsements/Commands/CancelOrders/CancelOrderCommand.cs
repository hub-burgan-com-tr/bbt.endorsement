using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Endorsements.Commands.CancelOrders
{
    public class CancelOrderCommand : IRequest<Response<bool>>
    {
        public string orderId { get; set; }
    }

    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Response<bool>>
    {
        IZeebeService _zeebe;

        public CancelOrderCommandHandler(IZeebeService zeebe)
        {
            _zeebe = zeebe;
        }

        public async Task<Response<bool>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var response = await _zeebe.SendMessage(request.orderId.ToString(), "contract_approval_delete", "");
            return Response<bool>.Success(true, 200);
        }
    }
}
