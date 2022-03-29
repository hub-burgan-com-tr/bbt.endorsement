using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models;
using Domain.Enum;
using MediatR;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Endorsements.Commands.NewOrders
{
    public class NewOrderCommand : IRequest<Response<StartResponse>>
    {
        public StartRequest StartRequest { get; set; }
        public StartFormRequest StartFormRequest { get; set; }
        public Form FormType { get; set; }
    }

    public class NewOrderCommandHandler : IRequestHandler<NewOrderCommand, Response<StartResponse>>
    {
        private readonly IZeebeService _zeebe;
        private readonly IApplicationDbContext _context;

        public NewOrderCommandHandler(IZeebeService zeebe, IApplicationDbContext context)
        {
            _zeebe = zeebe;
            _context = context;
        }

        public async Task<Response<StartResponse>> Handle(NewOrderCommand request, CancellationToken cancellationToken)
        {
            var instanceId = request.StartRequest != null ? request.StartRequest.Id : request.StartFormRequest.Id;

            var model = new ContractModel();
            if (request.FormType == Form.Order)
            {
                model = new ContractModel
                {
                    StartRequest = request.StartRequest,
                    FormType = request.FormType,
                    Device = false,
                    InstanceId = instanceId,
                    ExpireInMinutes = request.StartRequest.Config.ExpireInMinutes,
                    RetryFrequence = request.StartRequest.Config.RetryFrequence,
                    MaxRetryCount = request.StartRequest.Config.MaxRetryCount,
                };
            }
            else if (request.FormType == Form.FormOrder)
            {
                var config = _context.FormDefinitions.FirstOrDefault(x => x.FormDefinitionId == request.StartFormRequest.FormId);
                model = new ContractModel
                {
                    StartFormRequest = request.StartFormRequest,
                    FormType = request.FormType,
                    Device = false,
                    InstanceId = instanceId,
                    ExpireInMinutes = config.ExpireInMinutes,
                    RetryFrequence = config.RetryFrequence,
                    MaxRetryCount = config.MaxRetryCount,
                };
            }

            string payload = JsonSerializer.Serialize(model, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            var response = await _zeebe.SendMessage(instanceId.ToString(), "contact_approval_contract_new", payload);

            return Response<StartResponse>.Success(new StartResponse { InstanceId = instanceId }, 200);
        }
    }
}
