using Worker.App.Domain.Enums;

namespace Worker.App.Models;


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

    public class OrderDocument
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string FileName { get; set; }
        public ContentType Type { get; set; }
        public List<DocumentAction> Actions { get; set; }

        public class DocumentAction
        {
            public int Choice { get; set; }
            public string Title { get; set; }
            public ActionType Type { get; set; }
        }
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
        public int? MaxRetryCount { get; set; }
        public int RetryFrequence { get; set; }
        public int? ExpireInMinutes { get; set; }
        public string NotifyMessageSMS { get; set; }
        public string NotifyMessagePush { get; set; }
        public string RenotifyMessageSMS { get; set; }
        public string RenotifyMessagePush { get; set; }
    }
    public enum ContentType { HTML, PDF, PlainText }
    public enum CalbackMode { Completed, Verbose }
}