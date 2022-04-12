using Application.BbtInternals.Models;
using Application.Common.Models;
using MediatR;
using Newtonsoft.Json;
using RestSharp;

namespace Application.BbtInternals.Queries.GetSearchPersonSummary
{
    public class GetSearchPersonSummaryQuery :IRequest<Response<GetSearchPersonSummaryResponse>>
    {
        public string Name { get; set; }
    }

    public class GetSearchPersonSummaryQueryHandler : IRequestHandler<GetSearchPersonSummaryQuery, Response<GetSearchPersonSummaryResponse>>
    {
        public async Task<Response<GetSearchPersonSummaryResponse>> Handle(GetSearchPersonSummaryQuery request, CancellationToken cancellationToken)
        {
            var restClient = new RestClient("http://20.31.226.131:5000");
            var restRequest = new RestRequest("/Person", Method.Get);
            restRequest.AddParameter("name", request.Name, ParameterType.QueryString);
            var response = await restClient.ExecuteAsync(restRequest);
            var data = JsonConvert.DeserializeObject<List<PersonResponse>>(response.Content);

            var persons = data.Select(x => new GetSearchPersonSummaryDto
            {
                CitizenshipNumber = x.CitizenshipNumber,
                First = x.Name.First,
                Last = x.Name.Last
            });
            return Response<GetSearchPersonSummaryResponse>.Success(new GetSearchPersonSummaryResponse { Persons = persons }, 200);
        }
    }

}
