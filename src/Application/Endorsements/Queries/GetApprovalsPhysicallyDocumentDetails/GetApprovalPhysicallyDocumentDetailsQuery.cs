using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Endorsements.Commands.NewOrders;
using Domain.Enum;
using MediatR;

namespace Application.Endorsements.Queries.GetApprovalsPhysicallyDocumentDetails
{
    public class GetApprovalPhysicallyDocumentDetailsQuery : IRequest<Response<List<GetApprovalPhysicallyDocumentDetailsDto>>>
    {
        /// <summary>
        /// Onay Id
        /// </summary>
        public string OrderId { get; set; }
    }
    public class GetApprovalDocumentDetailsQueryHandler : IRequestHandler<GetApprovalPhysicallyDocumentDetailsQuery, Response<List<GetApprovalPhysicallyDocumentDetailsDto>>>
    {
        private IApplicationDbContext _context;
        public GetApprovalDocumentDetailsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<GetApprovalPhysicallyDocumentDetailsDto>>> Handle(GetApprovalPhysicallyDocumentDetailsQuery request, CancellationToken cancellationToken)
        {
            var response = _context.Documents.Where(x => x.OrderId == request.OrderId && x.Type == ContentType.PDF.ToString()).Select(x => new GetApprovalPhysicallyDocumentDetailsDto {DocumentId=x.DocumentId, Title = x.Order.Title, Name = x.Name, Actions = x.Actions.Select(x => new Action {ActionId=x.ActionId, IsDefault = x.IsDefault, Title = x.Title }).ToList() }).ToList();
            return Response<List<GetApprovalPhysicallyDocumentDetailsDto>>.Success(response, 200);
        }
    }
}
