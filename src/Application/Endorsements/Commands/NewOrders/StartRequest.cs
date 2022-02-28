using Domain.Enum;

namespace Application.Endorsements.Commands.NewOrders;

public class StartRequest
{
    /// <summary>
    /// Unique Id of order. Id is corrolation key of workflow also. 
    /// </summary>
    public Guid Id { get; set; }
    public OrderConfig Config { get; set; }
    public ReferenceClass Reference { get; set; }
    public long Customer { get; set; }
    public long Approver { get; set; }
    public DocumentClass[] Documents { get; set; }
    public class DocumentClass
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public ContentType Type { get; set; }

        public ActionClass[] Actions { get; set; }

        public class ActionClass
        {
            public bool IsDefault { get; set; }
            public string Title { get; set; }
            public ActionType Type { get; set; }
        }
    }

    public class ReferenceClass
    {
        public string Process { get; set; }
        public string State { get; set; }
        public Guid Id { get; set; }
        public CallbackClass Callback { get; set; }
        public class CallbackClass
        {
            public CalbackMode Mode { get; set; }
            public string URL { get; set; }
        }
    }

    public class OrderConfig
    {
        public int MaxRetryCount { get; set; }
        public string RetryFrequence { get; set; }
        public int ExpireInMinutes { get; set; }
        public string NotifyMessageSMS { get; set; }
        public string NotifyMessagePush { get; set; }
        public string RenotifyMessageSMS { get; set; }
        public string RenotifyMessagePush { get; set; }
    }
}
