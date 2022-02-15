using Newtonsoft.Json;

namespace Application.Approvals.Commands.CreateApprovalCommands;

public partial class CreateApprovalRequest
{
    public string InstanceId { get; set; }

    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("config")]
    public Config Config { get; set; }

    [JsonProperty("reference")]
    public Reference Reference { get; set; }

    [JsonProperty("customer")]
    public long Customer { get; set; }

    [JsonProperty("approvers")]
    public List<Approver> Approvers { get; set; }

    [JsonProperty("documents")]
    public List<Document> Documents { get; set; }
}

public partial class Approver
{
    [JsonProperty("user")]
    public long User { get; set; }

    [JsonProperty("order")]
    public string Order { get; set; }
}

public partial class Config
{
    [JsonProperty("max-retry-count")]
    public long MaxRetryCount { get; set; }

    [JsonProperty("retry-frequence")]
    public string RetryFrequence { get; set; }

    [JsonProperty("timeout-minutes")]
    public long TimeoutMinutes { get; set; }

    [JsonProperty("notify-message-transaction-sms")]
    public string NotifyMessageTransactionSms { get; set; }

    [JsonProperty("notify-message-transaction-push")]
    public string NotifyMessageTransactionPush { get; set; }

    [JsonProperty("re-notify-message-transaction-sms")]
    public string ReNotifyMessageTransactionSms { get; set; }

    [JsonProperty("re-notify-message-transaction-push")]
    public string ReNotifyMessageTransactionPush { get; set; }
}

public partial class Document
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("content")]
    public string Content { get; set; }

    [JsonProperty("actions")]
    public List<Action> Actions { get; set; }

    [JsonProperty("file-name", NullValueHandling = NullValueHandling.Ignore)]
    public string FileName { get; set; }
}

public partial class Action
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }
}

public partial class Reference
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("state")]
    public string State { get; set; }

    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("callback")]
    public Callback Callback { get; set; }
}

public partial class Callback
{
    [JsonProperty("mode")]
    public string Mode { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }
}
