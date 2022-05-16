using Newtonsoft.Json;
using RestSharp;
using Worker.App.Models;

namespace Worker.App.Infrastructure.Services;


public interface IMessagingService
{
    Task<SendSmsResponse> SendSmsMessageAsync(SendSmsRequest request);
    Task<SendMailResponse> SendMailMessageAsync(SendMailRequest request);
}

public class MessagingService : IMessagingService
{

    public Task<SendMailResponse> SendMailMessageAsync(SendMailRequest request)
    {
        var url = "https://test-messaginggateway.burgan.com.tr";
        var restClient = new RestClient(url);
        var restRequest = new RestRequest("/api/v1/Messaging/email/message", Method.Post);
        restRequest.RequestFormat = DataFormat.Json;
        restRequest.AddJsonBody(request);

        var response = restClient.ExecuteAsync(restRequest).Result;
        var data = JsonConvert.DeserializeObject<SendMailResponse>(response.Content);

        return Task.FromResult(data);
    }

    public Task<SendSmsResponse> SendSmsMessageAsync(SendSmsRequest request)
    {
        var url = "https://test-messaginggateway.burgan.com.tr";
        var restClient = new RestClient(url);
        var restRequest = new RestRequest("/api/v1/Messaging/sms/message", Method.Post);
        restRequest.RequestFormat = DataFormat.Json;
        restRequest.AddJsonBody(request);

        var response = restClient.ExecuteAsync(restRequest).Result;
        var data = JsonConvert.DeserializeObject<SendSmsResponse>(response.Content);

        throw new NotImplementedException();
    }
}
