using Application.BbtInternals.Models;
using Application.Common.Interfaces;
using Application.Common.Models;
using Newtonsoft.Json;
using RestSharp;

namespace Infrastructure.InternalsServices
{
    public class InternalsService : IInternalsService
    {
        private string internalsUrl;
        public InternalsService()
        {
            internalsUrl = StaticValues.Internals;
        }
        //kullanılmıyor
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
        //CustomerSearch & "person-search
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
        //person-get
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
            var data = JsonConvert.DeserializeObject<CustomerResponse>(response.Content);
            // return Response<List<CustomerList>>.Success(data.CustomerList.ToList(), 200);
            return null;
        }
    }
}
