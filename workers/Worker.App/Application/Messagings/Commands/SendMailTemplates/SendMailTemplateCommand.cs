using MediatR;
using Microsoft.EntityFrameworkCore;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;
using Worker.App.Infrastructure.Services;
using Worker.App.Models;

namespace Worker.App.Application.Messagings.Commands.SendMailTemplates;

public class SendMailTemplateCommand : IRequest<Response<bool>>
{
    public string OrderId { get; set; }
    public string Email { get; set; }
}

public class SendMailTemplateCommandHandler : IRequestHandler<SendMailTemplateCommand, Response<bool>>
{
    private IApplicationDbContext _context;
    private IDateTime _dateTime;
    private IMessagingService _messagingService = null!;

    public SendMailTemplateCommandHandler(IApplicationDbContext context, IDateTime dateTime, IMessagingService messagingService)
    {
        _context = context;
        _dateTime = dateTime;
        _messagingService = messagingService;
    }

    public async Task<Response<bool>> Handle(SendMailTemplateCommand request, CancellationToken cancellationToken)
    {
        var order = _context.Orders.Include(x => x.Documents).FirstOrDefault(x => x.OrderId == request.OrderId);
        if (order == null)
            return Response<bool>.NotFoundException("Order not found: " + request.OrderId, 404);

        var sendMailTemplate = new SendMailTemplateRequest
        {
            headerInfo = new SendMailTemplateHeaderInfo
            {
                sender = "Burgan"
            },
            template= "Müşteriye Giden Başvuru Onay Talebi",
            email = "HToremen@burgan.com.tr", // request.Email
            process = new SendMailTemplateProcess
            {
                name = "Zeebe - Contract Approval - SendOtp"
            }
        };
        
        await _messagingService.SendMailTemplateAsync(sendMailTemplate);

        return Response<bool>.Success(200);
    }
}

