using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Endorsements.Commands.NewOrders;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            var order = _context.Orders.Include(x => x.Documents).ThenInclude(x => x.Actions).Where(x => x.OrderId == request.OrderId).FirstOrDefault();
            var response = new GetApprovalDetailsDto
            {
                Title = order.Title,
                Documents = order.Documents.Where(y => y.Type.ToString() == ContentType.PlainText.ToString()).Select(y=>new OrderDocument { Name=y.Name,Content=y.Content,DocumentId=y.DocumentId,Actions=y.Actions.Select(z=>new Action { ActionId=z.ActionId,IsDefault=z.IsDefault,Title=z.Title}).ToList()}).ToList()};
            return Response<GetApprovalDetailsDto>.Success(response, 200);
        }
    }
}
