using Dms.Integration.Api.Models.Messages;
using Newtonsoft.Json;
using RestSharp;

namespace Dms.Integration.Api.Services;


public interface IMessagingService
{
    Task<SendMailResponse> SendMailTemplateAsync(SendEmailTemplateRequest request);
    Task<MessagingResponse> SendSmsMessageAsync(MessagingRequest request);
    Task<SendMailResponse> SendMailMessageAsync(SendMailRequest request);
}

public class MessagingService : IMessagingService
{
    public Task<SendMailResponse> SendMailTemplateAsync(SendEmailTemplateRequest request)
    {
        var url = "https://test-messaginggateway.burgan.com.tr";
        var restClient = new RestClient(url);
        var restRequest = new RestRequest("/api/v1/Messaging/email/templated", Method.Post);
        restRequest.RequestFormat = DataFormat.Json;
        restRequest.AddJsonBody(request);

        var response = restClient.ExecuteAsync(restRequest).Result;
        var data = JsonConvert.DeserializeObject<SendMailResponse>(response.Content);

        return Task.FromResult(data);
    }

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

    public Task<MessagingResponse> SendSmsMessageAsync(MessagingRequest request)
    {
        var url = "https://test-messaginggateway.burgan.com.tr";
        var restClient = new RestClient(url);
        var restRequest = new RestRequest("/api/v1/Messaging/sms/message", Method.Post);
        restRequest.RequestFormat = DataFormat.Json;
        restRequest.AddJsonBody(request);

        var response = restClient.ExecuteAsync(restRequest).Result;
        var data = JsonConvert.DeserializeObject<MessagingResponse>(response.Content);

        return Task.FromResult(data);
    }
}