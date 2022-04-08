namespace Worker.App.Application.Workers.Commands.ApproveContracts
{
    public class ApproveContractResponse
    {
        public IEnumerable<GetOrderDocumentState> Documents { get; set; }
    }

    public class GetOrderDocumentState
    {
        public string DocumentId { get; set; }
        public string State { get; set; }
    }
}
