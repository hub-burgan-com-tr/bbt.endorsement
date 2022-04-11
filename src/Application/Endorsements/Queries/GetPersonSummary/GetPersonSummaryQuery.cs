using Application.Common.Models;
using Application.Endorsements.Queries.GetSearchPersonSummary;
using MediatR;
using Newtonsoft.Json;
using RestSharp;

namespace Application.Endorsements.Queries.GetPersonSummary
{
    public class GetPersonSummaryQuery : IRequest<Response<GetSearchPersonSummaryResponse>>
    {
        public long Id { get; set; }
    }

        public class GetSearchPersonSummaryQueryHandler : IRequestHandler<GetPersonSummaryQuery, Response<GetSearchPersonSummaryResponse>>
        {
            public async Task<Response<GetSearchPersonSummaryResponse>> Handle(GetPersonSummaryQuery request, CancellationToken cancellationToken)
            {
                var restClient = new RestClient("http://20.31.226.131:5000");
                var restRequest = new RestRequest("/Person", Method.Get);
                restRequest.AddParameter("/", request.Id, ParameterType.QueryString);
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


