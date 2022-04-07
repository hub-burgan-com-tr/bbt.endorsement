namespace Application.Endorsements.Commands.NewOrders
{
    public class StartRequest
    {
        /// <summary>
        /// Unique Id of order. Id is corrolation key of workflow also. 
        /// </summary>
        public Guid Id { get; set; }
        public string Title { get; set; }
        public OrderConfig Config { get; set; }
        public OrderReference Reference { get; set; }
        public List<OrderDocument> Documents { get; set; }
        public OrderApprover Approver { get; set; }
    }

    public class OrderApprover
    {
        public long CitizenshipNumber { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
    }
    public class OrderDocument
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string FileName { get; set; }
        public int Type { get; set; }
        public List<DocumentAction> Actions { get; set; }

    }
    public class DocumentAction
    {
        public int Choice { get; set; }
        public string Title { get; set; }
        public ActionType Type { get; set; }
    }

   
    public class OrderReference
    {
        public string Process { get; set; }
        public string State { get; set; }
        public string ProcessNo { get; set; }
        public CallbackClass Callback { get; set; }

    }
    public class CallbackClass
    {
        public CalbackMode Mode { get; set; }
        public string URL { get; set; }
    }
    public class OrderConfig
    {
        public int MaxRetryCount { get; set; }
        public int RetryFrequence { get; set; }
        public int ExpireInMinutes { get; set; }
        public string NotifyMessageSMS { get; set; }
        public string NotifyMessagePush { get; set; }
        public string RenotifyMessageSMS { get; set; }
        public string RenotifyMessagePush { get; set; }
    }
    public enum ContentType { HTML, PDF, PlainText }
    public enum CalbackMode { Completed, Verbose }
    public enum ActionType { Approve=1, Reject=2 }

    public class OrderApprover_
    {
        public long ClientNumber { get; set; }
        public long CitizenshipNumber { get; set; }
        public NameClass Name { get; set; }
        public GsmPhone[] GsmPhones { get; set; }
        public string[] Emails { get; set; }
        public Device[] Devices { get; set; }

        public class NameClass
        {
            public string First { get; set; }
            public string Last { get; set; }
        }
        public class Device
        {
            public string DeviceId { get; set; }
            public string Label { get; set; }
        }
        public class GsmPhone
        {
            public int County { get; set; }
            public long Prefix { get; set; }
            public long Number { get; set; }
        }

    }


}
