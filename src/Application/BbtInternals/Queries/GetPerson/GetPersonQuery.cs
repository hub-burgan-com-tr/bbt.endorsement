using Application.BbtInternals.Models;
using Application.BbtInternals.Queries.GetSearchPersonSummary;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.BbtInternals.Queries.GetPerson
{
    public class GetPersonQuery : IRequest<Response<PersonResponse>>
    {
        public long Id { get; set; }
    }

    public class GetPersonQueryHandler : IRequestHandler<GetPersonQuery, Response<PersonResponse>>
    {
        private IInternalsService _internalsService;

        public GetPersonQueryHandler(IInternalsService internalsService)
        {
            _internalsService = internalsService;
        }
        public async Task<Response<PersonResponse>> Handle(GetPersonQuery request, CancellationToken cancellationToken)
        {
            var response = await _internalsService.GetPersonById(request.Id);
            if (response.Data == null)
                return Response<PersonResponse>.Fail("Pesponse.Data NULL", 201);

            return Response<PersonResponse>.Success(response.Data, 200);
        }
    }
}
