using Microsoft.AspNetCore.SignalR;

namespace Worker.Api.Hubs
{
    public class EventHub : Hub
    {
        private readonly ILogger<EventHub> _logger;

        public EventHub(ILogger<EventHub> logger)
        {
            _logger = logger;
        }

        public async Task Publish(string connectionId, string command, string state, Dictionary<string, object> payload)
        {
            await Task.Run(() =>
            {
                string message = string.Format("Command '{0}' received from {1} @ '{2}' state", command, connectionId, command, state);
                _logger.LogInformation(message);
            });


        }
    }
}
