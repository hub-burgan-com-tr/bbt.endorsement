using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Notification.Web.SignalR;

public interface IServerEventService
{
    void Register(Uri url);
    void PublishEvent(string connectionId, string command, string state, Dictionary<string, object> payload);
}

public class ServerEventService : IServerEventService
{
    private readonly ILogger<ServerEventService> _logger;
    private HubConnection hubConnection;

    public ServerEventService(ILogger<ServerEventService> logger)
    {
        _logger = logger;
    }

    public async void PublishEvent(string connectionId, string command, string state, Dictionary<string, object> payload)
    {
        while (hubConnection == null)
        {
            Thread.Sleep(100);
        }
        await hubConnection.SendAsync("Publish", connectionId, command, state, payload);
    }

    public async void Register(Uri url)
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl(url)
            .Build();

        await hubConnection.StartAsync();
    }
}