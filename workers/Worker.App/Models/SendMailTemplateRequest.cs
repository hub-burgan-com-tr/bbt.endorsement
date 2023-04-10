﻿namespace Worker.App.Models;

public class SendMailTemplateRequest
{
    public HeaderInfo headerInfo { get; set; }
    public string templateParams { get; set; }
    public string template { get; set; }
    public long customerNo { get; set; }
    public string email { get; set; }
    public Process process { get; set; }
}
public class SendMailTemplateProcess
{
    public string name { get; set; }
    public string itemId { get; set; }
    public string action { get; set; }
    public string identity { get; set; }
}
public class SendMailTemplateHeaderInfo
{
    public string sender { get; set; }
    public string smsPrefix { get; set; }
    public string smsSuffix { get; set; }
    public string emailTemplatePrefix { get; set; }
    public string emailTemplateSuffix { get; set; }
    public string smsTemplatePrefix { get; set; }
    public string smsTemplateSuffix { get; set; }
}

public class EmailTemplateParams
{
    public long MusteriNo { get; set; }
    public string MusteriAdSoyad { get; set; }
    public string Title { get; set; }
}