using MediatR;
using Worker.App.Application.Coomon.Models;

namespace Worker.App.Application.Workers.Commands.SendOtps
{
    public class SendOtpCommand : IRequest<Response<SendOtpResponse>>
    {
    }

    public class SendOtpCommandHandler : IRequestHandler<SendOtpCommand, Response<SendOtpResponse>>
    {
        public async Task<Response<SendOtpResponse>> Handle(SendOtpCommand request, CancellationToken cancellationToken)
        {
            return Response<SendOtpResponse>.Success(200);
        }
    }
}
