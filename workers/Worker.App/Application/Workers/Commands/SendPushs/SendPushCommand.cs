using MediatR;
using Worker.App.Application.Coomon.Models;

namespace Worker.App.Application.Workers.Commands.SendPushs
{
    public class SendPushCommand : IRequest<Response<SendPushResponse>>
    {
    }

    public class SendPushCommandHandler : IRequestHandler<SendPushCommand, Response<SendPushResponse>>
    {
        public async Task<Response<SendPushResponse>> Handle(SendPushCommand request, CancellationToken cancellationToken)
        {
            return Response<SendPushResponse>.Success(200);
        }
    }
}
