using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        try
        {
            var order = _context.Orders
                .Include(x=> x.Customer)
                .FirstOrDefault(x => x.OrderId == request.OrderId);

            if (order == null)
                return Response<MessageResponse>.NotFoundException("Order not found: " + request.OrderId, 404);

            var parameters = new EmailTemplateParams
            {
                MusteriAdSoyad = order.Customer.FirstName + " " + order.Customer.LastName,
                MusteriNo = order.Customer.CustomerNumber,
                Title = order.Title
            };
            var templateParams = JsonConvert.SerializeObject(parameters);
            var sendMailTemplate = new SendMailTemplateRequest
            {
                headerInfo = new SendMailTemplateHeaderInfo
                {
                    sender = "AutoDetect"
                },
                templateParams = templateParams,
                email = request.Email,
                process = new SendMailTemplateProcess
                {
                    name = "Zeebe - Contract Approval - SendOtp"
                }
            };

            var messages = new MessageResponse();

            if (order.State == OrderState.Approve.ToString())
            {
                sendMailTemplate.template = "Onaylandığına ilişkin PY ye Giden E-posta İçeriği:";
                var messageResponse = await _messagingService.SendMailTemplateAsync(sendMailTemplate);
                messages = new MessageResponse { Request = JsonConvert.SerializeObject(sendMailTemplate), Response = JsonConvert.SerializeObject(messageResponse) };
            }
            else if (order.State == OrderState.Reject.ToString())
            {
                sendMailTemplate.template = "Onaylanmadığına ilişkin PY ye Giden E-posta İçeriği:";
                var messageResponse = await _messagingService.SendMailTemplateAsync(sendMailTemplate);
                messages = new MessageResponse { Request = JsonConvert.SerializeObject(sendMailTemplate), Response = JsonConvert.SerializeObject(messageResponse) };
            }
            return Response<MessageResponse>.Success(messages, 200);
        }
        catch (Exception ex)
        {
            return Response<MessageResponse>.Fail(ex.Message, 201);
        }
    }
}