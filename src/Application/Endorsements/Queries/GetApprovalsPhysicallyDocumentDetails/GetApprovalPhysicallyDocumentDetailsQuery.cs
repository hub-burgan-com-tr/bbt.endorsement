using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Endorsements.Commands.NewOrders;
using Domain.Enum;
using MediatR;

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
            var response = _context.Documents.Where(x => x.Order.OrderId == request.OrderId && x.Type == DocumentTypeEnum.Physically.ToString()).Select(x => new GetApprovalPhysicallyDocumentDetailsDto { Name = x.Name, Actions = x.Actions.Where(x => x.Type == ContentType.PDF.ToString()).Select(x => new Action { IsDefault = x.IsDefault, Title = x.Title }).ToList() }).FirstOrDefault();
            return Response<GetApprovalPhysicallyDocumentDetailsDto>.Success(response, 200);
        }
    }
}
