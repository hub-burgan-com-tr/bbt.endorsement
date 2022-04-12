using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.BbtInternals.Queries.GetSearchPersonSummary
{
    public class GetSearchPersonSummaryQuery :IRequest<Response<GetSearchPersonSummaryResponse>>
    {
        public string Name { get; set; }
    }

    public class GetSearchPersonSummaryQueryHandler : IRequestHandler<GetSearchPersonSummaryQuery, Response<GetSearchPersonSummaryResponse>>
    {
        private IInternalsService _internalsService;

        public GetSearchPersonSummaryQueryHandler(IInternalsService internalsService)
        {
            _internalsService = internalsService;
        }

        public async Task<Response<GetSearchPersonSummaryResponse>> Handle(GetSearchPersonSummaryQuery request, CancellationToken cancellationToken)
        {
            var response = await _internalsService.GetPersonSearch(request.Name);
            var persons = response.Data.Select(x => new GetSearchPersonSummaryDto
            {
                CitizenshipNumber = x.CitizenshipNumber,
                First = x.Name.First,
                Last = x.Name.Last
            });
            return Response<GetSearchPersonSummaryResponse>.Success(new GetSearchPersonSummaryResponse { Persons = persons }, 200);
        }
    }

}
