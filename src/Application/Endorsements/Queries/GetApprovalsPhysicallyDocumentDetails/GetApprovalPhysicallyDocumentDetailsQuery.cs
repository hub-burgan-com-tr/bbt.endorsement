using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Endorsements.Commands.NewOrders;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Endorsements.Queries.GetApprovalsPhysicallyDocumentDetails
{
    public class GetApprovalPhysicallyDocumentDetailsQuery : IRequest<Response<GetApprovalPhysicallyDocumentDetailsDto>>
    {
        /// <summary>
        /// Onay Id
        /// </summary>
        public string OrderId { get; set; }
    }
    public class GetApprovalDocumentDetailsQueryHandler : IRequestHandler<GetApprovalPhysicallyDocumentDetailsQuery, Response<GetApprovalPhysicallyDocumentDetailsDto>>
    {
        private IApplicationDbContext _context;
        public GetApprovalDocumentDetailsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<GetApprovalPhysicallyDocumentDetailsDto>> Handle(GetApprovalPhysicallyDocumentDetailsQuery request, CancellationToken cancellationToken)
        {
            var order = _context.Orders.Include(x => x.Documents).ThenInclude(x => x.DocumentActions).Where(x => x.OrderId == request.OrderId).FirstOrDefault();
            var response = new GetApprovalPhysicallyDocumentDetailsDto
            {
                Title = order.Title,
                Documents = order.Documents.Where(y => y.Type.ToString() == ContentType.PDF.ToString()).Select(y => new OrderDocument { Name = y.Name, Content = y.Content, DocumentId = y.DocumentId, DocumentActions = y.DocumentActions.Select(z => new DocumentAction { DocumentActionId = z.DocumentActionId, IsDefault = z.IsDefault, Title = z.Title }).ToList() }).ToList()
            };
            return Response<GetApprovalPhysicallyDocumentDetailsDto>.Success(response, 200);
        }
    }
}
