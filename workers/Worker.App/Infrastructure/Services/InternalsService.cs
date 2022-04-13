using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;
using Worker.App.Application.Internals.Models;
using Worker.App.Infrastructure.Configuration.Options;

namespace Worker.App.Infrastructure.InternalsServices
{
    public class InternalsService : IInternalsService
    {
        private readonly AppSettings _appSetting;
        private readonly string internalsUrl;
        public InternalsService(IOptions<AppSettings> appSetting)
        {
            _appSetting = appSetting.Value;
            internalsUrl = "http://20.31.226.131:5000"; // _appSetting.Entegration.Internals;
        }

        public async Task<Response<PersonResponse>> GetPersonById(long id)
        {
            var restClient = new RestClient(internalsUrl);
            var restRequest = new RestRequest("/Person/" + id, Method.Get);
            var response = restClient.ExecuteAsync(restRequest).Result;
            var data = JsonConvert.DeserializeObject<PersonResponse>(response.Content);
            return Response<PersonResponse>.Success(data, 200);
        }

        public async Task<Response<List<PersonResponse>>> GetPersonSearch(string name)
        {
            var restClient = new RestClient(internalsUrl);
            var restRequest = new RestRequest("/Person", Method.Get);
            restRequest.AddParameter("name", name, ParameterType.QueryString);
            var response = await restClient.ExecuteAsync(restRequest);
            var data = JsonConvert.DeserializeObject<List<PersonResponse>>(response.Content);
            return Response<List<PersonResponse>>.Success(data, 200);
        }
    }
}
