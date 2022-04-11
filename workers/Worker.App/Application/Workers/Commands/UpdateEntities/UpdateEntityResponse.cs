using Worker.App.Domain.Enums;

namespace Worker.App.Application.Workers.Commands.UpdateEntities
{
    public class UpdateEntityResponse
    {
        public OrderState OrderState { get; set; }
        public bool IsUpdated { get; set; }
    }
}
