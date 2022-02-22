using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models;
using MediatR;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Application.Models.ContractModel;

namespace Application.Endorsements.Commands.NewOrders
{
    public class NewOrderCommand : IRequest<Response<StartResponse>>
    {
        public StartRequest StartRequest { get; set; }
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
            var model = new ContractModel
            {
                StartRequest = request.StartRequest,
                Device = false,
                InstanceId = Guid.NewGuid()
            };

            string payload = JsonSerializer.Serialize(model, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            var response = await _zeebe.SendMessage(model.InstanceId.ToString(), "contact_approval_contract_new", payload);

            return Response<StartResponse>.Success(new StartResponse { InstanceId = model.InstanceId }, 200);
        }
    }
}
