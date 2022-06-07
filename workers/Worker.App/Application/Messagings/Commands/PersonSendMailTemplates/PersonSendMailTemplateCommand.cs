using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;
using Worker.App.Infrastructure.Services;
using Worker.App.Models;

namespace Worker.App.Application.Messagings.Commands.PersonSendMailTemplates;

public class PersonSendMailTemplateCommand : IRequest<Response<List<MessageResponse>>>
{
    public string OrderId { get; set; }
    public string Email { get; set; }
}

public class PersonSendMailTemplateCommandHandler : IRequestHandler<PersonSendMailTemplateCommand, Response<List<MessageResponse>>>
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

    public async Task<Response<List<MessageResponse>>> Handle(PersonSendMailTemplateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = _context.Orders.Include(x => x.Documents).FirstOrDefault(x => x.OrderId == request.OrderId);
            if (order == null)
                return Response<List<MessageResponse>>.NotFoundException("Order not found: " + request.OrderId, 404);

            var names = order.Documents.Select(x => x.Name).ToArray();
            var emailTemplateParams = new EmailTemplateParams
            {
                MusteriAdSoyad = "Hüseyin Töremen",
                MusteriNo = 12345,
                Names = names
            };
            var templateParams = JsonConvert.SerializeObject(emailTemplateParams);
            var sendMailTemplate = new SendMailTemplateRequest
            {
                headerInfo = new SendMailTemplateHeaderInfo
                {
                    sender = "Burgan"
                },
                templateParams = templateParams,
                email = "HToremen@burgan.com.tr", // request.Email
                process = new SendMailTemplateProcess
                {
                    name = "Zeebe - Contract Approval - SendOtp"
                }
            };

            var messages = new List<MessageResponse>();

            if (order.State == OrderState.Approve.ToString())
            {
                sendMailTemplate.template = "Onaylandığına ilişkin PY ye Giden E-posta İçeriği:";
                var messageResponse = await _messagingService.SendMailTemplateAsync(sendMailTemplate);
                messages.Add(new MessageResponse { Request = JsonConvert.SerializeObject(sendMailTemplate), Response = JsonConvert.SerializeObject(messageResponse) });
            }
            else if (order.State == OrderState.Reject.ToString())
            {
                sendMailTemplate.template = "Onaylanmadığına ilişkin PY ye Giden E-posta İçeriği:";
                var messageResponse = await _messagingService.SendMailTemplateAsync(sendMailTemplate);
                messages.Add(new MessageResponse { Request = JsonConvert.SerializeObject(sendMailTemplate), Response = JsonConvert.SerializeObject(messageResponse) });
            }
            return Response<List<MessageResponse>>.Success(messages, 200);
        }
        catch (Exception ex)
        {
            return Response<List<MessageResponse>>.Fail(ex.Message, 201);
        }
    }
}