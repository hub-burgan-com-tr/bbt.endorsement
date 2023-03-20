using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Text.Json.Serialization;
using System.Text.Json;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;
using Worker.App.Application.Internals.Models;
using Serilog;

namespace Worker.App.Application.Workers.Commands.ConsumeCallback
{
    public class ConsumeCallbackCommand : IRequest<Response<ConsumeCallbackResponse>>
    {
        public string OrderId { get; set; }
    }

    public class ConsumeCallbackCommandHandler : IRequestHandler<ConsumeCallbackCommand, Response<ConsumeCallbackResponse>>
    {
        private IInternalsService _internalsService;
        private IApplicationDbContext _context;

        public ConsumeCallbackCommandHandler(IInternalsService internalsService, IApplicationDbContext context)
        {
            _internalsService = internalsService;
            _context = context;
        }

        public async Task<Response<ConsumeCallbackResponse>> Handle(ConsumeCallbackCommand request, CancellationToken cancellationToken)
        {
            var callback = _context.Callbacks.FirstOrDefault(x => x.OrderId == request.OrderId);
            if (callback == null)
                return Response<ConsumeCallbackResponse>.NotFoundException("callback not found", 404);

            var orderInfo = await _context.Orders
                .Include(x => x.Customer)
                .Include(x => x.Documents)
                .ThenInclude(x => x.DocumentActions)
                .Include(x => x.OrderHistories)
                .Where(x => x.OrderId == request.OrderId)
                .Select(x => new
                {
                    x.OrderId,

                    x.Title,
                    DisplayName = x.Customer.FirstName + " " + x.Customer.LastName,
                    x.Reference.Process,
                    x.Reference.State,
                    x.Reference.ProcessNo,
                    x.Config.MaxRetryCount,
                    x.Config.RetryFrequence,
                    x.Config.ExpireInMinutes,
                    OrderState = x.State,

                    History = x.OrderHistories.Where(x => x.IsStaff).OrderByDescending(x => x.Created)
                            .Select(x => new { CreatedDate = x.Created.ToString("dd.MM.yyyy HH:mm"), Description = x.Description, State = x.State }).ToList(),
                    Documents = x.Documents.OrderByDescending(x => x.Created).Select(x => new
                    {
                        x.DocumentId,
                        x.Name,
                        FileName = x.Name + ".pdf"
                    ,
                        x.MimeType,
                        TypeName = x.Type == ContentType.PlainText.ToString() ? "Metin" : "Belge",
                        x.DocumentActions.FirstOrDefault(y => y.IsSelected).Title,
                        Type = x.FileType
                    }).ToList()
                }).FirstOrDefaultAsync();

            if (orderInfo == null)
                return Response<ConsumeCallbackResponse>.NotFoundException("Order not found", 404);
            try
            {
                var client = new RestClient(callback.Url);
                var requestRest = new RestRequest(callback.Url, Method.Post);
                if (!string.IsNullOrEmpty(callback.ApiKey))
                {
                    requestRest.AddHeader("Authorization", callback.ApiKey);
                }
                var orderInfoJson = JsonSerializer.Serialize(orderInfo, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
                Log.ForContext("CalbackBody", orderInfoJson).Information($"ConsumeCallback");
                requestRest.AddHeader("Content-Type", "application/json");
                requestRest.AddStringBody(orderInfoJson, DataFormat.Json);
                Log.ForContext("CalbackRequestRest", JsonSerializer.Serialize(requestRest, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } })).Information($"ConsumeCallback");

                RestResponse responseRest = await client.ExecuteAsync(requestRest);

                return Response<ConsumeCallbackResponse>.Success(new ConsumeCallbackResponse { Content = responseRest.Content }, 200);

            }
            catch (Exception ex)
            {
                return Response<ConsumeCallbackResponse>.NotFoundException(ex.Message, 417);
            }


        }
    }
}
