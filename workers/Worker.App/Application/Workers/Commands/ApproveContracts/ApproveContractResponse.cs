using Worker.App.Domain.Enums;

namespace Worker.App.Application.Workers.Commands.ApproveContracts
{
    public class ApproveContractResponse
    {
        public OrderState OrderState { get; set; }
    }
}
