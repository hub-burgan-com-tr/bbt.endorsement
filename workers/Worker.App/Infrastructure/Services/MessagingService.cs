using Newtonsoft.Json;
using RestSharp;
using Worker.App.Models;

namespace Worker.App.Infrastructure.Services;


public interface IMessagingService
{
    Task<MessagingResponse> SendSmsMessageAsync(MessagingRequest request);
}

public class MessagingService : IMessagingService
{
    public Task<MessagingResponse> SendSmsMessageAsync(MessagingRequest request)
    {
        var url = "https://test-messaginggateway.burgan.com.tr";
        var restClient = new RestClient(url);
        var restRequest = new RestRequest("/api/v1/Messaging/sms/message", Method.Post);
        restRequest.RequestFormat = DataFormat.Json;
        restRequest.AddJsonBody(request);

        var response = restClient.ExecuteAsync(restRequest).Result;
        var data = JsonConvert.DeserializeObject<MessagingResponse>(response.Content);

        throw new NotImplementedException();
    }
}
