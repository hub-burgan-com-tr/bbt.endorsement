

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Infrastructure.Configuration.Options;
using Zeebe.Client;
using Zeebe.Client.Api.Responses;

namespace Worker.App.Infrastructure.Services.ZeebeServices
{
    public class ZeebeService : IZeebeService
    {
        private readonly IZeebeClient client;
        // private readonly ILogger<ZeebeService> _logger;
        //private readonly IClientEventService _clientEventService;
        //private readonly IServerEventService _serverEventService;

        private string ZeebeUrl;

        public ZeebeService(IConfiguration configuration)
        {
            // _logger = logger;
            var settings = configuration.Get<AppSettings>();
            ZeebeUrl = settings.Zeebe.ZeebeGateway.ToString();

            client = ZeebeClient.Builder()
                                .UseLoggerFactory(new LoggerFactory())
                                .UseGatewayAddress(ZeebeUrl)
                                .UsePlainText()
                                .Build();
        }

        public async Task<string> SendMessage(string instanceId, string messageName, string payload)
        {
            // _logger.LogInformation("instanceId: " + instanceId);
            await client.NewPublishMessageCommand()
                .MessageName(messageName)
                .CorrelationKey(instanceId)
                .TimeToLive(TimeSpan.FromSeconds(10))
                .Variables(payload)
                .Send();

            string jsonText = JsonSerializer.Serialize(instanceId, new JsonSerializerOptions
            { Converters = { new JsonStringEnumConverter() } });

            return jsonText;
        }


        public void StartWorkers(string url)
        {
            Uri baseUri = new Uri(url);
            //_clientEventService.Register(baseUri);
            //_serverEventService.Register(baseUri);
        }

        public async Task<IDeployResourceResponse> Deploy(string modelFilename)
        {
            var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Processes", modelFilename);
            var deployment = await client.NewDeployCommand().AddResourceFile(filename).Send();
            return deployment;
        }

        public Task<ITopology> Status()
        {
            return client.TopologyRequest().Send();
        }

        public IZeebeClient Client()
        {
            return client;
        }
    }
}
