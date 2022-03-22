using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Endorsements.Commands.NewOrders;
using Domain.Enum;
using MediatR;

namespace Application.Endorsements.Queries.GetApprovalsDetails
{
    public class GetApprovalDetailsQuery : IRequest<Response<GetApprovalDetailsDto>>
    {
        /// <summary>
        /// Onay Id
        /// </summary>
        public string OrderId { get; set; }
    }
    /// <summary>
    /// Onayimdakiler Detay Sayfasi
    /// </summary>
    public class GetApprovalDetailQueryHandler : IRequestHandler<GetApprovalDetailsQuery, Response<GetApprovalDetailsDto>>
    {
        private IApplicationDbContext _context;
        public GetApprovalDetailQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<GetApprovalDetailsDto>> Handle(GetApprovalDetailsQuery request, CancellationToken cancellationToken)
        {
            var response = _context.Documents.Where(x => x.OrderId == request.OrderId && x.Type == ContentType.PlainText.ToString()).Select(x => new GetApprovalDetailsDto {Title=x.Order.Title,Name=x.Name,Content=x.Content,Actions=x.Actions.Select(x=>new Action { IsDefault=x.IsDefault,Title=x.Title}).ToList() }).FirstOrDefault();
            return Response<GetApprovalDetailsDto>.Success(response, 200);
        }
    }
}
