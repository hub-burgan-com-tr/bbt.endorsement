using Newtonsoft.Json;
using RestSharp;
using Worker.App.Application.Common.Models;
using Worker.App.Models;

namespace Worker.App.Infrastructure.Services;


public interface IMessagingService
{
    Task<MessageResponse> SendSmsMessageAsync(SendSmsRequest request);
    Task<MessageResponse> SendMailMessageAsync(SendMailRequest request);

    Task<MessageResponse> SendMailTemplateAsync(SendMailTemplateRequest request);
    Task<MessageResponse> SendSmsTemplateAsync(SendSmsTemplateRequest request);
}

public class MessagingService : IMessagingService
{
    private string messagingGateway = "";
    public MessagingService()
    {
        messagingGateway = StaticValues.MessagingGateway;
    }

    public Task<MessageResponse> SendMailMessageAsync(SendMailRequest request)
    {
        var restClient = new RestClient(messagingGateway);
        var restRequest = new RestRequest("/api/v1/Messaging/email/message", Method.Post);
        restRequest.RequestFormat = DataFormat.Json;
        restRequest.AddJsonBody(request);

        var response = restClient.ExecuteAsync(restRequest).Result;
        var data = JsonConvert.DeserializeObject<MessageResponse>(response.Content);

        return Task.FromResult(data);
    }

    public Task<MessageResponse> SendMailTemplateAsync(SendMailTemplateRequest request)
    {
        var restClient = new RestClient(messagingGateway);
        var restRequest = new RestRequest("/api/v1/Messaging/email/templated", Method.Post);
        restRequest.RequestFormat = DataFormat.Json;
        restRequest.AddJsonBody(request);

        var response = restClient.ExecuteAsync(restRequest).Result;
        var data = JsonConvert.DeserializeObject<MessageResponse>(response.Content);

        return Task.FromResult(data);
    }

    public Task<MessageResponse> SendSmsMessageAsync(SendSmsRequest request)
    {
        var restClient = new RestClient(messagingGateway);
        var restRequest = new RestRequest("/api/v1/Messaging/sms/message", Method.Post);
        restRequest.RequestFormat = DataFormat.Json;
        restRequest.AddJsonBody(request);

        var response = restClient.ExecuteAsync(restRequest).Result;
        var data = JsonConvert.DeserializeObject<MessageResponse>(response.Content);

        throw new NotImplementedException();
    }

    public Task<MessageResponse> SendSmsTemplateAsync(SendSmsTemplateRequest request)
    {
        var restClient = new RestClient(messagingGateway);
        var restRequest = new RestRequest("/api/v1/Messaging/sms/templated", Method.Post);
        restRequest.RequestFormat = DataFormat.Json;
        restRequest.AddJsonBody(request);

        var response = restClient.ExecuteAsync(restRequest).Result;
        var data = JsonConvert.DeserializeObject<MessageResponse>(response.Content);

        return Task.FromResult(data);
    }
}
