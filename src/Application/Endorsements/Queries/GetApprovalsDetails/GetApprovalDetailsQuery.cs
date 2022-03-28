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
            var response = _context.Orders
                .Where(x => x.OrderId == request.OrderId)
                .Select(x => new GetApprovalDetailsDto
                {
                    Title = x.Title,
                    Documents = x.Documents.Select(y => new OrderDocument
                    {
                        Content = y.Content,
                        DocumentId = y.DocumentId,
                        Name = y.Name,
                        Actions = y.DocumentActions.Select(z => new Action
                        {
                            ActionId = z.DocumentActionId,
                            IsDefault = z.IsDefault,
                            Title = z.Title
                        }).ToList()
                    }).ToList()
                }).FirstOrDefault();

            return Response<GetApprovalDetailsDto>.Success(response, 200);
        }
    }
}
