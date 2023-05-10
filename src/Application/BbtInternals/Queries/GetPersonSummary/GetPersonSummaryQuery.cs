using Application.BbtInternals.Queries.GetSearchPersonSummary;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.BbtInternals.Queries.GetPersonSummary
{
    public class GetPersonSummaryQuery : IRequest<Response<GetSearchPersonSummaryResponse>>
    {
        public long Id { get; set; }
    }

    public class GetSearchPersonSummaryQueryHandler : IRequestHandler<GetPersonSummaryQuery, Response<GetSearchPersonSummaryResponse>>
    {
        private IInternalsService _internalsService;

        public GetSearchPersonSummaryQueryHandler(IInternalsService internalsService)
        {
            _internalsService = internalsService;
        }
        public async Task<Response<GetSearchPersonSummaryResponse>> Handle(GetPersonSummaryQuery request, CancellationToken cancellationToken)
        {
            var response = await _internalsService.GetPersonById(request.Id);

            if (response.Data == null)
                return Response<GetSearchPersonSummaryResponse>.Fail("Pesponse.Data NULL", 201);

            var person = new GetSearchPersonSummaryDto
            {
                CitizenshipNumber = response.Data.CitizenshipNumber.ToString(),
                First = response.Data.Name.First,
                Last = response.Data.Name.Last
            };
            return Response<GetSearchPersonSummaryResponse>.Success(new GetSearchPersonSummaryResponse { Person = person }, 200);
        }
    }

}


