using Domain.Enums;

namespace Worker.App.Application.Workers.Commands.ApproveContracts
{
    public class ApproveContractResponse
    {
        public OrderState OrderState { get; set; }
        public List<ApproveContractDocumentResponse> Documents { get; set; }
    }

    public class ApproveContractDocumentResponse
    {
        public string DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string ActionTitle { get; set; }
    }
}
