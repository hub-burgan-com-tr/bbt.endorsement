namespace Application.Endorsements.Commands.NewOrderForms
{
    public class NewOrderFormResponse
    {
        /// <summary>
        /// Unique Id of order. Id is corrolation key of workflow also. 
        /// </summary>
        public Guid InstanceId { get; set; }
    }
}
