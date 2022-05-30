using Newtonsoft.Json;
using RestSharp;
using Worker.App.Application.Common.Models;
using Worker.App.Models;

namespace Worker.App.Infrastructure.Services;


public interface IMessagingService
{
    Task<SendSmsResponse> SendSmsMessageAsync(SendSmsRequest request);
    Task<SendMailResponse> SendMailMessageAsync(SendMailRequest request);

    Task<SendMailTemplateResponse> SendMailTemplateAsync(SendMailTemplateRequest request);
}

public class MessagingService : IMessagingService
{
    private string messagingGateway = "";
    public MessagingService()
    {
        messagingGateway = StaticValues.MessagingGateway;
    }

    public Task<SendMailResponse> SendMailMessageAsync(SendMailRequest request)
    {
        var restClient = new RestClient(messagingGateway);
        var restRequest = new RestRequest("/api/v1/Messaging/email/message", Method.Post);
        restRequest.RequestFormat = DataFormat.Json;
        restRequest.AddJsonBody(request);

        var response = restClient.ExecuteAsync(restRequest).Result;
        var data = JsonConvert.DeserializeObject<SendMailResponse>(response.Content);

        return Task.FromResult(data);
    }

    public Task<SendMailTemplateResponse> SendMailTemplateAsync(SendMailTemplateRequest request)
    {
        var restClient = new RestClient(messagingGateway);
        var restRequest = new RestRequest("/api/v1/Messaging/email/templated", Method.Post);
        restRequest.RequestFormat = DataFormat.Json;
        restRequest.AddJsonBody(request);

        var response = restClient.ExecuteAsync(restRequest).Result;
        var data = JsonConvert.DeserializeObject<SendMailTemplateResponse>(response.Content);

        return Task.FromResult(data);
    }

    public Task<SendSmsResponse> SendSmsMessageAsync(SendSmsRequest request)
    {
        var restClient = new RestClient(messagingGateway);
        var restRequest = new RestRequest("/api/v1/Messaging/sms/message", Method.Post);
        restRequest.RequestFormat = DataFormat.Json;
        restRequest.AddJsonBody(request);

        var response = restClient.ExecuteAsync(restRequest).Result;
        var data = JsonConvert.DeserializeObject<SendSmsResponse>(response.Content);

        throw new NotImplementedException();
    }
}
