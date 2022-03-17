using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Endorsements.Commands.NewOrders;
using MediatR;

namespace Application.Endorsements.Queries.GetApprovalsDocumentList
{
    public class GetApprovalsDocumentListQuery:IRequest<Response<List<GetApprovalsDocumentListDto>>>
    {
        public string OrderId { get; set; }
    }

    public class GetApprovalsDocumentListQueryHandler : IRequestHandler<GetApprovalsDocumentListQuery, Response<List<GetApprovalsDocumentListDto>>>
    {
        private IApplicationDbContext _context;

        public GetApprovalsDocumentListQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<GetApprovalsDocumentListDto>>> Handle(GetApprovalsDocumentListQuery request, CancellationToken cancellationToken)
        {
            var list = _context.Documents.Where(x => x.Order.OrderId == request.OrderId).Select(x => new GetApprovalsDocumentListDto
            { Name = x.Name, 
                PlainTextActions = x.Actions.Where(x => x.Document.Type== ContentType.PlainText.ToString())
                .Select(x => new Action { IsDefault = x.IsDefault, Title = x.Title,Type=x.Type }).ToList() ,
                HTMLActions = x.Actions.Where(x => x.Document.Type == ContentType.HTML.ToString())
                .Select(x => new Action { IsDefault = x.IsDefault, Title = x.Title,Type=x.Type }).ToList(),
                PDFActions = x.Actions.Where(x => x.Document.Type == ContentType.PDF.ToString())
                .Select(x => new Action { IsDefault = x.IsDefault, Title = x.Title,Type=x.Type }).ToList()
            }).ToList();
            return Response<List<GetApprovalsDocumentListDto>>.Success(list, 200);
        }
    }
}
