using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models;
using MediatR;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Endorsements.Commands.DocumentApprovals
{
    public class ApproveDataCommand : IRequest<Response<ApproveDataResponse>>
    {
        public Guid OrderId { get; set; }
        public Guid DocumentId { get; set; }
    }

    public class ApproveDataCommandHandler : IRequestHandler<ApproveDataCommand, Response<ApproveDataResponse>>
    {
        IZeebeService _zeebe;

        public ApproveDataCommandHandler(IZeebeService zeebe)
        {
            _zeebe = zeebe;
        }

        public async Task<Response<ApproveDataResponse>> Handle(ApproveDataCommand request, CancellationToken cancellationToken)
        {
            var model = new ContractModel
            {
                InstanceId = request.OrderId,
                
            };

            string payload = JsonSerializer.Serialize(model, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            var response = await _zeebe.SendMessage(model.InstanceId.ToString(), "ApproveData", payload);

            return Response<ApproveDataResponse>.Success(200);
        }
    }
}
