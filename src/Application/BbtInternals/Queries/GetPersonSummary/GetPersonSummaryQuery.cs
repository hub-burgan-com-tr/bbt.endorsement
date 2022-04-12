using Application.BbtInternals.Queries.GetSearchPersonSummary;
using Application.Common.Models;
using MediatR;
using Newtonsoft.Json;
using RestSharp;

namespace Application.BbtInternals.Queries.GetPersonSummary
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
            var restRequest = new RestRequest("/Person/" + request.Id, Method.Get);
                var response = await restClient.ExecuteAsync(restRequest);
                var data = JsonConvert.DeserializeObject<PersonResponse>(response.Content);

                var person = new GetSearchPersonSummaryDto
                {
                    CitizenshipNumber = data.CitizenshipNumber,
                    First = data.Name.First,
                    Last = data.Name.Last
                };
                return Response<GetSearchPersonSummaryResponse>.Success(new GetSearchPersonSummaryResponse { Person = person }, 200);
            }
        }

    }


