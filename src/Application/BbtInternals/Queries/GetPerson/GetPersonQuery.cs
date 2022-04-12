using Application.BbtInternals.Models;
using Application.Common.Models;
using MediatR;
using Newtonsoft.Json;
using RestSharp;

namespace Application.BbtInternals.Queries.GetPerson
{
    public class GetPersonQuery : IRequest<Response<PersonResponse>>
    {
        public long Id { get; set; }
    }

    public class GetPersonQueryHandler : IRequestHandler<GetPersonQuery, Response<PersonResponse>>
    {
        public async Task<Response<PersonResponse>> Handle(GetPersonQuery request, CancellationToken cancellationToken)
        {
            var restClient = new RestClient("http://20.31.226.131:5000");
            var restRequest = new RestRequest("/Person/" + request.Id, Method.Get);
            var response = await restClient.ExecuteAsync(restRequest);
            var data = JsonConvert.DeserializeObject<PersonResponse>(response.Content);
            return Response<PersonResponse>.Success(data, 200);
        }
    }
}
