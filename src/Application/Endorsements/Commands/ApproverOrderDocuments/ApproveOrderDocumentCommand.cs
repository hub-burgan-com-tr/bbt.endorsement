using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models;
using MediatR;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Endorsements.Commands.ApproveOrderDocuments
{
    public class ApproveOrderDocumentCommand : IRequest<Response<bool>>
    {
        public Guid OrderId { get; set; }

        public List<ApproveOrderDocument> Documents { get; set; }
    }

    public class ApproveOrderDocumentCommandHandler : IRequestHandler<ApproveOrderDocumentCommand, Response<bool>>
    {
        IZeebeService _zeebe;

        public ApproveOrderDocumentCommandHandler(IZeebeService zeebe)
        {
            _zeebe = zeebe;
        }

        public async Task<Response<bool>> Handle(ApproveOrderDocumentCommand request, CancellationToken cancellationToken)
        {
            var model = new ContractModel
            {
                InstanceId = request.OrderId,
                Documents = request.Documents               
            };
            string payload = JsonSerializer.Serialize(model, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });          
            var response = await _zeebe.SendMessage(model.InstanceId.ToString(), "ApproveData", payload);
            return Response<bool>.Success(200);
        }
    }
}
