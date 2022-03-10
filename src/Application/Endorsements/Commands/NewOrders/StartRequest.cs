using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace Application.Endorsements.Commands.NewOrders;

public class StartRequest
{
    public StartRequest()
    {
        this.Id = Guid.NewGuid();
    }

    /// <summary>
    /// Unique Id of order. Id is corrolation key of workflow also. 
    /// </summary>
    public Guid Id { get; set; }
    public string Title { get; set; }
    public OrderConfig Config { get; set; }
    public ReferenceClass Reference { get; set; }
    public DocumentClass[] Documents { get; set; }
    //public ApproverClass Approver { get; set; }

    public class DocumentClass
    {
        public int DocumentType { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        //public Dictionary<string, string> FormParameters { get; set; }

        //public List<Option> Options { get; set; }
        public IFormFile Files { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ContentType Type { get; set; }
        public List<ActionClass> Actions { get; set; } // Options
        public enum ContentType { HTML, PDF, PlainText }

        public class ActionClass
        {
            public bool IsDefault { get; set; }
            public string Title { get; set; }
            [JsonConverter(typeof(StringEnumConverter))]
            public ActionType Type { get; set; }
            public enum ActionType { Approve, Reject }
        }

        //public class Option
        //{
        //    public string Title { get; set; }
        //    public string Choice { get; set; }
        //}
    }
    public class ApproverClass
    {
        //public long Customer { get; set; }
        //public long Approver { get; set; }
        public int Type { get; set; }
        public string Value { get; set; }
        public string NameSurname { get; set; }
    }
    public class ReferenceClass
    {
        public string Process { get; set; }
        public string State { get; set; }
        public string ProcessNo { get; set; }
        public CallbackClass Callback { get; set; }
        
    }
    public class CallbackClass
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public CalbackMode Mode { get; set; }
        public string URL { get; set; }
        public enum CalbackMode { Completed, Verbose }
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
