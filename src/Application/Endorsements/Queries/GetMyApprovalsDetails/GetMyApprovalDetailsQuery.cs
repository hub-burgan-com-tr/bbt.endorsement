using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Endorsements.Commands.NewOrders;
using MediatR;

namespace Application.Endorsements.Queries.GetMyApprovalsDetails
{
    public class GetMyApprovalDetailsQuery : IRequest<Response<List<GetMyApprovalDetailsDto>>>
    {/// <summary>
    /// Onay Id
    /// </summary>
        public string OrderId { get; set; }

    }
    /// <summary>
    /// Onayladıklarım Detay Sayfası
    /// </summary>
    public class GetMyApprovalDetailQueryHandler : IRequestHandler<GetMyApprovalDetailsQuery, Response<List<GetMyApprovalDetailsDto>>>
    {
        private IApplicationDbContext _context;

        public GetMyApprovalDetailQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<GetMyApprovalDetailsDto>>> Handle(GetMyApprovalDetailsQuery request, CancellationToken cancellationToken)
        {

            var list = _context.Documents.Where(x => x.OrderId == request.OrderId).Select(x => new GetMyApprovalDetailsDto
            {
                Name = x.Name,
                PlainTextActions = x.Actions.Where(x => x.Document.Type == ContentType.PlainText.ToString())
                 .Select(x => new Action { IsDefault = x.IsDefault, Title = x.Title, Type = x.Type }).ToList(),
                HTMLActions = x.Actions.Where(x => x.Document.Type == ContentType.HTML.ToString())
                 .Select(x => new Action { IsDefault = x.IsDefault, Title = x.Title, Type = x.Type }).ToList(),
                PDFActions = x.Actions.Where(x => x.Document.Type == ContentType.PDF.ToString())
                 .Select(x => new Action { IsDefault = x.IsDefault, Title = x.Title, Type = x.Type }).ToList(),
                History=null
            }).ToList();
            return Response<List<GetMyApprovalDetailsDto>>.Success(list, 200);
        }
    }
}
