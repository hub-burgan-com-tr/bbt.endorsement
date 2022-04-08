using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Endorsements.Queries.GetMyApprovals
{
    public class GetMyApprovalDto : IMapFrom<Order>
    {/// <summary>
     /// Onay Id
     /// </summary>
        public string OrderId { get; set; }
    /// <summary>
    /// Onay Ad
    /// </summary>
        public string Title { get; set; }
    /// <summary>
    /// Belge Var Mi
    /// </summary>
        public bool IsDocument { get; set; }
    /// <summary>
    /// Onay Icon
    /// </summary>
        public string State { get; set; }
    }   
}
