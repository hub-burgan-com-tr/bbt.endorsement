namespace Dms.Integration.Api.Models.Messages;

public class SendMailRequest
{
    public string from { get; set; }
    public int customerNo { get; set; }
    public string subject { get; set; }
    public string content { get; set; }
    public string contactId { get; set; }
    public string id { get; set; }
    public string email { get; set; }
    public List<MailAttachment> attachments { get; set; }
    public MailProcess process { get; set; }
}

public class MailProcess
{
    public string name { get; set; }
    public string itemId { get; set; }
    public string action { get; set; }
    public string identity { get; set; }
}

public class MailAttachment
{
    public string name { get; set; }
    public string data { get; set; }
}

public class SendMailResponse
{
    public string txnId { get; set; }
    public string status { get; set; }
}