using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;
using Worker.App.Infrastructure.Services;
using Worker.App.Models;

namespace Worker.App.Application.Messagings.Commands.PersonSendMailTemplates;

public class PersonSendMailTemplateCommand : IRequest<Response<MessageResponse>>
{
    public string OrderId { get; set; }
    public string Email { get; set; }
}

public class PersonSendMailTemplateCommandHandler : IRequestHandler<PersonSendMailTemplateCommand, Response<MessageResponse>>
{
    private IApplicationDbContext _context;
    private IDateTime _dateTime;
    private IMessagingService _messagingService = null!;

    public PersonSendMailTemplateCommandHandler(IApplicationDbContext context, IDateTime dateTime, IMessagingService messagingService)
    {
        _context = context;
        _dateTime = dateTime;
        _messagingService = messagingService;
    }

    public async Task<Response<MessageResponse>> Handle(PersonSendMailTemplateCommand request, CancellationToken cancellationToken)
    {
        var messages = new MessageResponse();
        try
        {
            var order = _context.Orders
                .Include(x=> x.Person)
                .FirstOrDefault(x => x.OrderId == request.OrderId);

            if (order == null)
                return Response<MessageResponse>.NotFoundException("Order not found: " + request.OrderId, 404);

            messages.PersonId = order.PersonId;

            var param = new EmailTemplateParams
            {
                MusteriAdSoyad = order.Customer.FirstName + " " + order.Customer.LastName,
                MusteriNo = order.Customer.CustomerNumber,
                Title = order.Title
            };
            var templateParams = JsonConvert.SerializeObject(param);
            var sendMailTemplate = new SendMailTemplateRequest
            {
                headerInfo = new HeaderInfo
                {
                    sender = "AutoDetect"
                },
                templateParams = templateParams,
                customerNo = order.Person.CustomerNumber,
                email = request.Email,
                process = new Process
                {
                    name = "bbt.endorsement -  PersonalSendMail"
                }
            };

            if (order.State == OrderState.Approve.ToString())
            {
                sendMailTemplate.template = "Onaylandığına ilişkin PY ye Giden E-posta İçeriği";
                var messageResponse = await _messagingService.SendMailTemplateAsync(sendMailTemplate, request.OrderId);
                messages = new MessageResponse { Request = JsonConvert.SerializeObject(sendMailTemplate), Response = JsonConvert.SerializeObject(messageResponse) };
            }
            else if (order.State == OrderState.Reject.ToString())
            {
                sendMailTemplate.template = "Onaylanmadığına ilişkin PY ye Giden E-posta İçeriği:";
                var messageResponse = await _messagingService.SendMailTemplateAsync(sendMailTemplate, request.OrderId);
                messages = new MessageResponse { Request = JsonConvert.SerializeObject(sendMailTemplate), Response = JsonConvert.SerializeObject(messageResponse) };
            }
        }
        catch (Exception ex)
        {
            Log.ForContext("OrderId", request.OrderId).Error(ex, ex.Message);
            return Response<MessageResponse>.Fail(ex.Message, 201);
        }
        return Response<MessageResponse>.Success(messages, 200);
    }
}