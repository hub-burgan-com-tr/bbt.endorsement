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
            internalsUrl = StaticValues.Internals;
        }

       

        public async Task<Response<CustomerResponse>> GetCustomerSearchByName(CustomerSearchRequest request)
        {
            var restClient = new RestClient(internalsUrl);
            var restRequest = new RestRequest("/CustomerSearch", Method.Post);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Accept", "text/plain");

            restRequest.AddBody(request);
            var response = await restClient.ExecutePostAsync(restRequest);

            var data = JsonConvert.DeserializeObject<CustomerResponse>(response.Content);
            return Response<CustomerResponse>.Success(data, 200);
        }
        #region func
        public async Task<Response<CustomerResponse>> GetCustomerSearch(CustomerRequest person)
        {
            var restClient = new RestClient(internalsUrl);
            var restRequest = new RestRequest("/Customer", Method.Post);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Accept", "text/plain");

            restRequest.AddBody(person);
            var response = await restClient.ExecutePostAsync(restRequest);

            var data = JsonConvert.DeserializeObject<CustomerResponse>(response.Content);
            return Response<CustomerResponse>.Success(data, 200);
        }
        public async Task<Response<PersonResponse>> GetPersonById(long id)
        {
            try
            {
                var restClient = new RestClient(internalsUrl);
                var restRequest = new RestRequest("/Person/" + id, Method.Get);
                var response = restClient.ExecuteAsync(restRequest).Result;
                var data = JsonConvert.DeserializeObject<PersonResponse>(response.Content);
                return Response<PersonResponse>.Success(data, 200);
            }
            catch (Exception ex)
            {
                return Response<PersonResponse>.Fail(ex.Message, 201);
            }
        }

        public async Task<Response<List<PersonResponse>>> GetPersonSearch(string name)
        {
            try
            {
                var restClient = new RestClient(internalsUrl);
                var restRequest = new RestRequest("/Person", Method.Get);
                restRequest.AddParameter("name", name, ParameterType.QueryString);
                var response = restClient.ExecuteAsync(restRequest).Result;
                var data = JsonConvert.DeserializeObject<List<PersonResponse>>(response.Content);
                return Response<List<PersonResponse>>.Success(data, 200);
            }
            catch (Exception ex)
            {
                return Response<List<PersonResponse>>.Fail(ex.Message, 201);
            }
        }
        #endregion

    }
}
