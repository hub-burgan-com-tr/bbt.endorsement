using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Endorsements.Commands.NewOrders;
using Domain.Enum;
using MediatR;

namespace Application.Endorsements.Queries.GetApprovalsFormDocumentDetail
{
    public class GetApprovalFormDocumentDetailQuery : IRequest<Response<GetApprovalFormDocumentDetailDto>>
    {
        public string OrderId { get; set; }
    }

    public class GetApprovalsFormDocumentDetailQueryHandler : IRequestHandler<GetApprovalFormDocumentDetailQuery, Response<GetApprovalFormDocumentDetailDto>>
    {
        private IApplicationDbContext _context;
        public GetApprovalsFormDocumentDetailQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<GetApprovalFormDocumentDetailDto>> Handle(GetApprovalFormDocumentDetailQuery request, CancellationToken cancellationToken)
        {
            var response = _context.Documents.Where(x => x.Order.OrderId == request.OrderId && x.Type == DocumentTypeEnum.Form.ToString()).Select(x => new GetApprovalFormDocumentDetailDto { Name = x.Name, CitizenShipNumber = "", FirstAndSurname = "", Actions = x.Actions.Where(x => x.Type == ContentType.HTML.ToString()).Select(x => new Action { IsDefault = x.IsDefault, Title = x.Title }).ToList() }).FirstOrDefault(); return Response<GetApprovalFormDocumentDetailDto>.Success(response, 200);
        }
    }
}
