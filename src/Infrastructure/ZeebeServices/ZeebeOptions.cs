namespace Infrastructure.ZeebeServices
{
    public class ZeebeOptions
    {
        public string ModelFilename { get; set; }
        public string ProcessPath { get; set; }
        public string ZeebeGateway { get; set; }
        public string EventHubUrl { get; set; }
    }
}
