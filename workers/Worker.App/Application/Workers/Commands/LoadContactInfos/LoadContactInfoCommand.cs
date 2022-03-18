using MediatR;
using Worker.App.Application.Coomon.Models;

namespace Worker.App.Application.Workers.Commands.LoadContactInfos
{
    public class LoadContactInfoCommand : IRequest<Response<LoadContactInfoResponse>>
    {

    }

    public class LoadContactInfoCommandHandler : IRequestHandler<LoadContactInfoCommand, Response<LoadContactInfoResponse>>
    {
        public async Task<Response<LoadContactInfoResponse>> Handle(LoadContactInfoCommand request, CancellationToken cancellationToken)
        {
            return Response<LoadContactInfoResponse>.Success(200);
        }
    }
}
