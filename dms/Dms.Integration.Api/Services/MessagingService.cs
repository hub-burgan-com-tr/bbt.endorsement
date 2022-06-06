using Dms.Integration.Api.Models.Messages;
using Dms.Integration.Infrastructure.Extensions;
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
    private readonly string _url;
    public MessagingService(IConfigurationRoot config)
    {
        _url = config.GetMessagingGatewayUrl();
    }

    public Task<SendMailResponse> SendMailTemplateAsync(SendEmailTemplateRequest request)
    {
        var restClient = new RestClient(_url);
        var restRequest = new RestRequest("/api/v1/Messaging/email/templated", Method.Post);
        restRequest.RequestFormat = DataFormat.Json;
        restRequest.AddJsonBody(request);

        var response = restClient.ExecuteAsync(restRequest).Result;
        var data = JsonConvert.DeserializeObject<SendMailResponse>(response.Content);

        return Task.FromResult(data);
    }

    public Task<SendMailResponse> SendMailMessageAsync(SendMailRequest request)
    {
        var restClient = new RestClient(_url);
        var restRequest = new RestRequest("/api/v1/Messaging/email/message", Method.Post);
        restRequest.RequestFormat = DataFormat.Json;
        restRequest.AddJsonBody(request);

        var response = restClient.ExecuteAsync(restRequest).Result;
        var data = JsonConvert.DeserializeObject<SendMailResponse>(response.Content);

        return Task.FromResult(data);
    }

    public Task<MessagingResponse> SendSmsMessageAsync(MessagingRequest request)
    {
        var restClient = new RestClient(_url);
        var restRequest = new RestRequest("/api/v1/Messaging/sms/message", Method.Post);
        restRequest.RequestFormat = DataFormat.Json;
        restRequest.AddJsonBody(request);

        var response = restClient.ExecuteAsync(restRequest).Result;
        var data = JsonConvert.DeserializeObject<MessagingResponse>(response.Content);

        return Task.FromResult(data);
    }
}