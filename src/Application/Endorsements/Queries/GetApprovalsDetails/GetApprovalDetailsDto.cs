using Application.Endorsements.Commands.NewOrders;

namespace Application.Endorsements.Queries.GetApprovalsDetails
{
    public class GetApprovalDetailsDto
    {
       
        public string Name { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// Belge Onay
        /// </summary>
        public List<Action> Actions { get; set; }
        public string Title { get; internal set; }
        public string DocumentId { get; internal set; }
    }
    public class Action
    {
        public bool IsDefault { get; set; }
        public string Title { get; set; }
        public string ActionId { get; internal set; }
    }
   
}
