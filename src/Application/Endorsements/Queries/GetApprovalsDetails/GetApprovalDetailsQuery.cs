using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Endorsements.Commands.NewOrders;
using Domain.Enum;
using MediatR;

namespace Application.Endorsements.Queries.GetApprovalsDetails
{
    public class GetApprovalDetailsQuery : IRequest<Response<List<GetApprovalDetailsDto>>>
    {
        /// <summary>
        /// Onay Id
        /// </summary>
        public string OrderId { get; set; }
    }
    /// <summary>
    /// Onayimdakiler Detay Sayfasi
    /// </summary>
    public class GetApprovalDetailQueryHandler : IRequestHandler<GetApprovalDetailsQuery, Response<List<GetApprovalDetailsDto>>>
    {
        private IApplicationDbContext _context;
        public GetApprovalDetailQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<List<GetApprovalDetailsDto>>> Handle(GetApprovalDetailsQuery request, CancellationToken cancellationToken)
        {
            var response = _context.Documents.Where(x => x.OrderId == request.OrderId && x.Type == ContentType.PlainText.ToString()).Select(x => new GetApprovalDetailsDto {DocumentId=x.DocumentId, Title = x.Order.Title, Name = x.Name, Content = x.Content, Actions = x.Actions.Select(x => new Action {ActionId=x.ActionId, IsDefault = x.IsDefault, Title = x.Title }).ToList() }).ToList();
            return Response<List<GetApprovalDetailsDto>>.Success(response, 200);
        }
    }
}
