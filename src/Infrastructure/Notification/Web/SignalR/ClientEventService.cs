using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Notification.Web.SignalR;


public delegate void ClientEvent(string command, string state, Dictionary<string, object> payload);

public interface IClientEventService
{
    void Register(Uri url);
    string ConnectionId();
    void SubcribeMeForClientEvent(ClientEvent callback);
}

public class ClientEventService : IClientEventService
{
    private readonly ILogger<ClientEventService> _logger;
    private HubConnection hubConnection;
    private List<ClientEvent> ClientEventSubscribers = new List<ClientEvent>();
    private readonly NavigationManager _navigationManager;

    public ClientEventService(ILogger<ClientEventService> logger)
    {
        _logger = logger;
    }

    public string ConnectionId()
    {
        return hubConnection.ConnectionId;
    }

    public async void Register(Uri url)
    {
        hubConnection = new HubConnectionBuilder()
               .WithUrl(url)
               .Build();

        hubConnection.On<string, string, Dictionary<string, object>>("FireClientEvent", (command, state, payload) =>
        {
            string message = string.Format("FireClientEvent is catched - State : {0} and Command: {1} with payload {2} ", state, command, payload);
            _logger.LogInformation(message);

            ClientEventSubscribers.ForEach(s => s.Invoke(command, state, payload));
        });

        await hubConnection.StartAsync();
    }

    public void SubcribeMeForClientEvent(ClientEvent callback)
    {
        ClientEventSubscribers.Add(callback);
    }
}