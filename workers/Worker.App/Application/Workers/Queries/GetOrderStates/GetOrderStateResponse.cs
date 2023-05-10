using Domain.Enums;

namespace Worker.App.Application.Workers.Queries.GetOrderStates;

public class GetOrderStateResponse
{
    public OrderState OrderState { get; set; }
}