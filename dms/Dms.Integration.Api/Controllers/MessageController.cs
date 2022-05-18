using Dms.Integration.Api.Models.Messages;
using Dms.Integration.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dms.Integration.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly IMessagingService _messagingService;

    public MessageController(IMessagingService messagingService)
    {
        this._messagingService = messagingService;
    }

    [Route("send-sms-message")]
    [HttpPost]
    public async Task<MessagingResponse> SendSmsMessage(MessagingRequest message)
    {
        var messageRequest = new MessagingRequest
        {
            headerInfo = new HeaderInfo
            {
                sender = "On"
            },
            content = @"Değerli Müşterimiz, ""belgeonay.burgan.com.tr"" linkine giriş yapıp, başvurunuza ilişkin belgeleri ""Onayımdakiler"" adımından onaylamanızı rica ederiz. Detaylı bilgi için 0 850 222 8 222 numaralı telefonumuzdan bizi arayabilirsiniz.  Mersis : 0140003231000116",
            contentType = "Private",
            phone = new Phone
            {
                countryCode = 90, // gsmPhone.County,
                prefix = 542, // gsmPhone.Prefix,
                number = 4729390, // gsmPhone.Number
            },
            customerNo = 20186951,
            smsType = "Fast",
            id = Guid.NewGuid().ToString(),
            process = new Process
            {
                name = "Zeebe - Contract Approval - SendOtp"
            }
        };
        var response = await _messagingService.SendSmsMessageAsync(messageRequest);

        return response;
    }

    [Route("send-email-message")]
    [HttpPost]
    public async Task<SendMailResponse> SendEmailMessage(SendMailRequest request)
    {
        var emailRequest = new SendMailRequest
        {
            id = Guid.NewGuid().ToString(),
            customerNo = 20186951,
            subject = "Zeebe - Contract Approval - SendMail",
            content = @"Değerli Müşterimiz, ""belgeonay.burgan.com.tr"" linkine giriş yapıp, başvurunuza ilişkin belgeleri ""Onayımdakiler"" adımından onaylamanızı rica ederiz. Detaylı bilgi için 0 850 222 8 222 numaralı telefonumuzdan bizi arayabilirsiniz.  Mersis : 0140003231000116",
            from = "noreplay@m.on.com.tr",
            email = "Htoremen@burganbank.com.tr",
            contactId = "",
            process = new MailProcess
            {
                name = "Zeebe - Contract Approval - SendMail"
            }
        };
        var response = await _messagingService.SendMailMessageAsync(emailRequest);

        return response;
    }
}