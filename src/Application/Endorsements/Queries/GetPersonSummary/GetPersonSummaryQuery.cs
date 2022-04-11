using Application.Common.Models;
using MediatR;
using RestSharp;

namespace Application.Endorsements.Queries.GetPersonSummary
{
    public class GetPersonSummaryQuery : IRequest<Response<GetPersonSummaryDto>>
    {
        public long Id { get; set; }
    }

    public class GetPersonSummaryQueryHandler : IRequestHandler<GetPersonSummaryQuery, Response<GetPersonSummaryDto>>
    {
       
        public async Task<Response<GetPersonSummaryDto>> Handle(GetPersonSummaryQuery request, CancellationToken cancellationToken)
        {
            var restClient = new RestClient("http://20.31.226.131:5000");

            var restRequest = new RestRequest(String.Format("{0}/{1}", "/Person",request.Id), Method.Get);
            var response = restClient.ExecuteGetAsync<GetPersonSummaryDto>(restRequest);
            return Response<GetPersonSummaryDto>.Success(new GetPersonSummaryDto { CitizenshipNumber = response.Result.Data.CitizenshipNumber, First = response.Result.Data.First, Last = response.Result.Data.Last }, 200);

        }
    }

}
