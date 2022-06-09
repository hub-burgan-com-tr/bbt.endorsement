namespace Worker.App.Models;

public class SendSmsTemplateRequest
{
    public HeaderInfo headerInfo { get; set; }
    public string templateParams { get; set; }
    public string template { get; set; }
    public string title { get; set; }
    public int customerNo { get; set; }
    public Phone phone { get; set; }
    public Process process { get; set; }
}