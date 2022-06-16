namespace Dms.Integration.Api.Models.Messages;

public class MessagingRequest
{
    public HeaderInfo headerInfo { get; set; }
    public int customerNo { get; set; }
    public string content { get; set; }
    public string contentType { get; set; }
    public string smsType { get; set; }
    public string contactId { get; set; }
    public string id { get; set; }
    public Phone phone { get; set; }
    public Process process { get; set; }
}

public class Process
{
    public string name { get; set; }
    public string itemId { get; set; }
    public string action { get; set; }
    public string identity { get; set; }
}

public class Phone
{
    public int countryCode { get; set; }
    public int prefix { get; set; }
    public int number { get; set; }
}

public class HeaderInfo
{
    public string sender { get; set; }
    public string smsPrefix { get; set; }
    public string smsSuffix { get; set; }
    public string emailTemplatePrefix { get; set; }
    public string emailTemplateSuffix { get; set; }
    public string smsTemplatePrefix { get; set; }
    public string smsTemplateSuffix { get; set; }
}

public class MessagingResponse
{
    public string txnId { get; set; }
    public string status { get; set; }
}