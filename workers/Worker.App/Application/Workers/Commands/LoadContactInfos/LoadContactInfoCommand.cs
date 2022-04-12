using MediatR;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;

namespace Worker.App.Application.Workers.Commands.LoadContactInfos
{
    public class LoadContactInfoCommand : IRequest<Response<LoadContactInfoResponse>>
    {
        public long Id { get; set; }
    }

    public class LoadContactInfoCommandHandler : IRequestHandler<LoadContactInfoCommand, Response<LoadContactInfoResponse>>
    {
        private IInternalsService _internalsService;

        public LoadContactInfoCommandHandler(IInternalsService internalsService)
        {
            _internalsService = internalsService;
        }

        public async Task<Response<LoadContactInfoResponse>> Handle(LoadContactInfoCommand request, CancellationToken cancellationToken)
        {
            var response = await _internalsService.GetPersonById(request.Id);
            return Response<LoadContactInfoResponse>.Success(new LoadContactInfoResponse { Person = response.Data }, 200);
        }
    }
}
