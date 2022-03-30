using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Endorsements.Commands.NewOrders;
using Application.Models;
using Domain.Enum;
using MediatR;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Endorsements.Commands.NewOrderForms
{
    public class NewOrderFormCommand : IRequest<Response<NewOrderFormResponse>>
    {
        public StartFormRequest Request { get; set; }
        public Form FormType { get; set; }
    }

    public class NewFormOrderCommandHandler : IRequestHandler<NewOrderFormCommand, Response<NewOrderFormResponse>>
    {
        private readonly IZeebeService _zeebe;
        private readonly IApplicationDbContext _context;

        public NewFormOrderCommandHandler(IZeebeService zeebe, IApplicationDbContext context)
        {
            _zeebe = zeebe;
            _context = context;
        }

        public async Task<Response<NewOrderFormResponse>> Handle(NewOrderFormCommand request, CancellationToken cancellationToken)
        {
            var instanceId = request.Request.Id;

            var config = _context.FormDefinitions.FirstOrDefault(x => x.FormDefinitionId == request.Request.FormId);
            var model = new ContractModel
            {
                StartFormRequest = request.Request,
                FormType = request.FormType,
                Device = false,
                InstanceId = instanceId,
                ExpireInMinutes = config.ExpireInMinutes,
                RetryFrequence = config.RetryFrequence,
                MaxRetryCount = config.MaxRetryCount,
            };
            
            string payload = JsonSerializer.Serialize(model, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            var response = await _zeebe.SendMessage(instanceId.ToString(), "contact_approval_contract_new", payload);

            return Response<NewOrderFormResponse>.Success(new NewOrderFormResponse { InstanceId = instanceId }, 200);

        }
    }
}
