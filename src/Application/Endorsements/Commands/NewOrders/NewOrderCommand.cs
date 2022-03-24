using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models;
using MediatR;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Endorsements.Commands.NewOrders
{
    public class NewOrderCommand : IRequest<Response<StartResponse>>
    {
        public StartRequest StartRequest { get; set; }
        public StartFormRequest StartFormRequest { get; set; }  
    }

    public class NewOrderCommandHandler : IRequestHandler<NewOrderCommand, Response<StartResponse>>
    {
        IZeebeService _zeebe;

        public NewOrderCommandHandler(IZeebeService zeebe)
        {
            _zeebe = zeebe;
        }

        public async Task<Response<StartResponse>> Handle(NewOrderCommand request, CancellationToken cancellationToken)
        {
            var instanceId = request.StartRequest != null ? request.StartRequest.Id : request.StartFormRequest.Id;
            var model = new ContractModel
            {
                StartRequest = request.StartRequest,
                StartFormRequest = request.StartFormRequest,
                Device = false,
                InstanceId = instanceId
            };

            string payload = JsonSerializer.Serialize(model, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            var response = await _zeebe.SendMessage(instanceId.ToString(), "contact_approval_contract_new", payload);

            return Response<StartResponse>.Success(new StartResponse { InstanceId = instanceId }, 200);
        }
    }
}
