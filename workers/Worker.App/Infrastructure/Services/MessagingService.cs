using Newtonsoft.Json;
using RestSharp;
using Serilog;
using Worker.App.Application.Common.Models;
using Worker.App.Models;

namespace Worker.App.Infrastructure.Services;


public interface IMessagingService
{
    Task<Response> SendSmsMessageAsync(SendSmsRequest request);
    Task<Response> SendMailMessageAsync(SendMailRequest request);

    Task<Response> SendMailTemplateAsync(SendMailTemplateRequestV2 request, string instanceId);
    Task<Response> SendSmsTemplateAsync(SendSmsTemplateRequest request);
}

public class MessagingService : IMessagingService
{
    private string messagingGateway = "";
    public MessagingService()
    {
        messagingGateway = StaticValues.MessagingGateway;
    }

    public Task<Response> SendMailMessageAsync(SendMailRequest request)
    {
        var restClient = new RestClient(messagingGateway);
        var restRequest = new RestRequest("/api/v1/Messaging/email/templated", Method.Post);
        restRequest.RequestFormat = DataFormat.Json;
        restRequest.AddJsonBody(request);

        var response = restClient.ExecuteAsync(restRequest).Result;
        var data = JsonConvert.DeserializeObject<Response>(response.Content);
        return Task.FromResult(data);
    }

    public Task<Response> SendMailTemplateAsync(SendMailTemplateRequestV2 request, string instanceId)
    {
        var restClient = new RestClient(messagingGateway);
        var restRequest = new RestRequest("/api/v2/Messaging/email/templated", Method.Post);
        restRequest.RequestFormat = DataFormat.Json;
        restRequest.AddJsonBody(request);

        var response = restClient.ExecuteAsync(restRequest).Result;
        if (response.Content != null)
            Log.ForContext("OrderId", instanceId).Information(response.Content.ToString());
        if (response.Content == "Template Not Found")
        {
            Log.ForContext("OrderId", instanceId).Information(response.Content.ToString());
            return Task.FromResult(new Response());
        }
        var data = JsonConvert.DeserializeObject<Response>(response.Content);

        return Task.FromResult(data);
    }

    public Task<Response> SendSmsMessageAsync(SendSmsRequest request)
    {
        var restClient = new RestClient(messagingGateway);
        var restRequest = new RestRequest("/api/v1/Messaging/sms/message", Method.Post);
        restRequest.RequestFormat = DataFormat.Json;
        restRequest.AddJsonBody(request);

        var response = restClient.ExecuteAsync(restRequest).Result;
        var data = JsonConvert.DeserializeObject<Response>(response.Content);

        return Task.FromResult(data);
    }

    public Task<Response> SendSmsTemplateAsync(SendSmsTemplateRequest request)
    {
        var restClient = new RestClient(messagingGateway);
        var restRequest = new RestRequest("/api/v1/Messaging/sms/templated", Method.Post);
        restRequest.RequestFormat = DataFormat.Json;
        restRequest.AddJsonBody(request);

        var response = restClient.ExecuteAsync(restRequest).Result;
        var data = JsonConvert.DeserializeObject<Response>(response.Content);

        return Task.FromResult(data);
    }
}
