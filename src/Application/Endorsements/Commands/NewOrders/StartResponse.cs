using FluentValidation.Results;

namespace Application.Endorsements.Commands.NewOrders
{
    public class StartResponse
    {
        /// <summary>
        /// Unique Id of order. Id is corrolation key of workflow also. 
        /// </summary>
        public string InstanceId { get; set; }
    }
}
