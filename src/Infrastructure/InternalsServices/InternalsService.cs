using Application.BbtInternals.Models;
using Application.Common.Interfaces;
using Application.Common.Models;
using Infrastructure.Configuration.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace Infrastructure.InternalsServices
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
            var response = await restClient.ExecuteAsync(restRequest);
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
