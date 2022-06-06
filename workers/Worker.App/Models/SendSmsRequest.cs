namespace Worker.App.Models;

public class SendSmsRequest
{
    public HeaderInfo headerInfo { get; set; }
    public int customerNo { get; set; }
    public string content { get; set; } //sms içeriği , loglarda critic bilgilerin maskelenmesi için bu alanların <mask></mask> tagları arasında gönderilmesi gerekmektedir.Örn “Şifreniz <mask>123456</mask> ile giriş yapabilirsiniz.”
    public string contentType { get; set; }
    public string smsType { get; set; } //Alabileceği değerler Fast,Bulk 
    public string id { get; set; }
    public Phone phone { get; set; }
    public Process process { get; set; }
}

public class HeaderInfo
{
    public string sender { get; set; } // Alabileceği Değerler (Burgan , On , AutoDetect) | Müşterinin BusinessLine’ı biliniyorsa Burgan ya da On gönderilmeli. AutoDetect seçilirse customer db’den bu bilgi alınır.
    public string smsPrefix { get; set; } // AutoDetect seçilmediyse Değer verilirse sms’in önüne bu ifade eklenir
    public string smsSuffix { get; set; } // AutoDetect seçilmediyse Değer verilirse sms’in arkasına bu ifade eklenir
    public string emailTemplatePrefix { get; set; } // AutoDetect seçilmediyse  Değer verilirse mail’in önüne bu ifade eklenir
    public string emailTemplateSuffix { get; set; } // AutoDetect seçilmediyse Değer verilirse mail’in önüne bu ifade eklenir
    public string smsTemplatePrefix { get; set; }
    public string smsTemplateSuffix { get; set; }
}


public class Phone
{
    public int countryCode { get; set; } //ülke kodu 90
    public int prefix { get; set; } //Gsm prefix 553,533 etc
    public int number { get; set; } // Telefon numarası
}

public class Process
{
    public string name { get; set; } //İşlem programatik tetikleniyorsa program bilgisi
    public string itemId { get; set; }
    public string action { get; set; }
    public string identity { get; set; } //İşlemi bir kullanıcı tetikliyorsa user code bilgisi
}

public class SendSmsResponse
{
    public string txnId { get; set; }
    public string status { get; set; }
}