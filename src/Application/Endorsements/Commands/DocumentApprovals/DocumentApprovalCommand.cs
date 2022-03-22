using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models;
using MediatR;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Endorsements.Commands.DocumentApprovals
{
    public class DocumentApprovalCommand : IRequest<Response<DocumentApprovalResponse>>
    {
        public Guid OrderId { get; set; }
        public Guid DocumentId { get; set; }
    }

    public class DocumentApprovalCommandHandler : IRequestHandler<DocumentApprovalCommand, Response<DocumentApprovalResponse>>
    {
        IZeebeService _zeebe;

        public DocumentApprovalCommandHandler(IZeebeService zeebe)
        {
            _zeebe = zeebe;
        }

        public async Task<Response<DocumentApprovalResponse>> Handle(DocumentApprovalCommand request, CancellationToken cancellationToken)
        {
            var model = new ContractModel
            {
                InstanceId = request.OrderId,
                
            };

            string payload = JsonSerializer.Serialize(model, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            var response = await _zeebe.SendMessage(model.InstanceId.ToString(), "ApproveData", payload);

            return Response<DocumentApprovalResponse>.Success(200);
        }
    }
}
