namespace Dms.Integration.Api.Models.Messages;

public class SendEmailTemplateRequest
{
    public HeaderInfo headerInfo { get; set; }
    public string templateParams { get; set; }
    public string template { get; set; }
    public string email { get; set; }
    public Process process { get; set; }
}

public class EmailTemplateParams
{
    //public int MusteriNo { get; set; }
    //public string MusteriAdSoyad { get; set; }
    public string Title { get; set; }
}