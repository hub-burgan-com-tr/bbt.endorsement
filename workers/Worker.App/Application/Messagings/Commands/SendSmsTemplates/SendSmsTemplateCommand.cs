using MediatR;
using Newtonsoft.Json;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;
using Worker.App.Application.Internals.Models;
using Worker.App.Application.Messagings.Commands.SendMailTemplates;
using Worker.App.Infrastructure.Services;
using Worker.App.Models;

namespace Worker.App.Application.Messagings.Commands.SendSmsTemplates;

public class SendSmsTemplateCommand : IRequest<Response<MessageResponse>>
{
    public string OrderId { get; set; }
    public GsmPhones GsmPhone { get; set; }
    public int CustomerNumber { get; set; }
}

public class SendSmsTemplateCommandHandler : IRequestHandler<SendSmsTemplateCommand, Response<MessageResponse>>
{
    private IApplicationDbContext _context;
    private IDateTime _dateTime;
    private IMessagingService _messagingService = null!;

    public SendSmsTemplateCommandHandler(IApplicationDbContext context, IDateTime dateTime, IMessagingService messagingService)
    {
        _context = context;
        _dateTime = dateTime;
        _messagingService = messagingService;
    }

    public async Task<Response<MessageResponse>> Handle(SendSmsTemplateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == request.OrderId);
            var gsmPhone = request.GsmPhone; 
            
            var param = new SendMailTemplate
            {
                Title = order.Title
            };
            var templateParams = JsonConvert.SerializeObject(param);

            var messageRequest = new SendSmsTemplateRequest
            {
                headerInfo = new HeaderInfo
                {
                    sender = "AutoDetect"
                },
                templateParams = templateParams,
                template = "Müşteriden Talep Edilen Onay SMS'i",
                title = order.Title,
                phone = new Phone
                {
                    countryCode = request.GsmPhone.County,
                    prefix = request.GsmPhone.Prefix,
                    number = request.GsmPhone.Number
                },
                //customerNo = request.CustomerNumber,
                process = new Process
                {
                    name = "Zeebe - bbt.endorsement",
                    //itemId = request.OrderId,
                    //action = "SendOtp"
                }
            };
            var response = await _messagingService.SendSmsTemplateAsync(messageRequest);

            var messageResponse = new MessageResponse { Request = JsonConvert.SerializeObject(messageRequest), Response = JsonConvert.SerializeObject(response), CustomerId = order.CustomerId };
            return Response<MessageResponse>.Success(messageResponse, 200);
        }
        catch (Exception ex)
        {
            return Response<MessageResponse>.Fail(ex.Message, 201);
        }
    }
}