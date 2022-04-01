using Domain.Enum;

namespace Application.Endorsements.Queries.GetOrderDetails;


public class OrderDetail
{
    /// <summary>
    /// Unique Id of order. Id is corrolation key of workflow also. 
    /// </summary>
    public Guid Id { get; set; }

    public long Customer { get; set; }
    public long Approver { get; set; }

    public OrderConfig Config { get; set; }

    public NotificationLog[] NotificationLogs { get; set; }
    public class NotificationLog
    {
        public Guid Id { get; set; }
        public DateTime At { get; set; }
        public string Message { get; set; }
        public string Channel { get; set; }
        public string Trigger { get; set; }
    }

    public DocumentClass[] Documents { get; set; }
    public class DocumentClass
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ContentType Type { get; set; }
        public enum ContentType { HTML, PDF, PlainText }
        public ActionClass[] Actions { get; set; }
        public class ActionClass
        {
            public int Choice { get; set; }
            public string Title { get; set; }
            public ActionType State { get; set; }
            public enum ActionType { Approved, Rejected }
        }
        public Log[] Logs { get; set; }
        public class Log
        {
            public Guid Id { get; set; }
            public DateTime At { get; set; }
            public string Device { get; set; }
            public enum LogType { Displayed, Approved, Rejected }
        }
    }

    public ReferenceClass Reference { get; set; }
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
            public enum CalbackMode { Completed, Verbose }

            public Log[] Logs { get; set; }
            public class Log
            {
                public Guid Id { get; set; }
                public DateTime At { get; set; }
                public int ResponseCode { get; set; }
                public string Response { get; set; }
            }
        }
    }

    public class OrderConfig
    {
        public int? MaxRetryCount { get; set; }
        public int RetryFrequence { get; set; }
        public int? ExpireInMinutes { get; set; }
        public string NotifyMessageSMS { get; set; }
        public string NotifyMessagePush { get; set; }
        public string RenotifyMessageSMS { get; set; }
        public string RenotifyMessagePush { get; set; }
    }
}