﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;
using Worker.App.Infrastructure.Services;
using Worker.App.Models;

namespace Worker.App.Application.Messagings.Commands.SendMailTemplates;

public class SendMailTemplateCommand : IRequest<Response<MessageResponse>>
{
    public string OrderId { get; set; }
    public string Email { get; set; }
}

public class SendMailTemplateCommandHandler : IRequestHandler<SendMailTemplateCommand, Response<MessageResponse>>
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

    public async Task<Response<MessageResponse>> Handle(SendMailTemplateCommand request, CancellationToken cancellationToken)
    {
        var messageResponse = new MessageResponse();
        try
        {
            var order = _context.Orders
                    .Include(x => x.Documents).Include(x=>x.Customer)
                    .FirstOrDefault(x => x.OrderId == request.OrderId);

            if (order == null)
                return Response<MessageResponse>.NotFoundException("Order not found: " + request.OrderId, 404);

            var param = new SendMailTemplate
            {
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
                customerNo = order.Customer.CustomerNumber,
                email = request.Email,
                template = "Müşteriye Giden Başvuru Onay Talebi",
                process = new Process
                {
                    name = "bbt.endorsement - SendOtp"
                }
            };

            var response = await _messagingService.SendMailTemplateAsync(sendMailTemplate, request.OrderId);
            messageResponse = new MessageResponse { Request = JsonConvert.SerializeObject(sendMailTemplate), Response = JsonConvert.SerializeObject(response) };
            messageResponse.CustomerId = order.CustomerId; 
            return Response<MessageResponse>.Success(messageResponse, 200);
        }
        catch (Exception ex)
        {        
            Log.ForContext("OrderId", request.OrderId).Error(ex, ex.Message);
            return Response<MessageResponse>.Fail(ex.Message, 201);
        }
    }
}

