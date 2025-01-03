namespace Worker.App.Application.Workers.Queries.GetOrderConfigs
{
    public class GetOrderConfigResponse
    {
        public string ExpireInMinutes { get; set; }
        public string RetryFrequence { get; set; }
        public int MaxRetryCount { get; set; }
        public bool Device { get; set; }
        public bool NotPersonalMail { get; set; }
        public bool NoNotification { get; set; }
        public bool UseContractManagement { get; set; }
        
    }
}
