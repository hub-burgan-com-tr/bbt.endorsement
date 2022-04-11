using Worker.App.Domain.Enums;

namespace Worker.App.Application.Workers.Commands.DeleteEntities
{
    public class DeleteEntityResponse
    {
        public OrderState OrderState { get; set; }
        public bool IsUpdated { get; set; }
    }
}
