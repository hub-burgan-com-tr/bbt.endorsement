using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Endorsements.Commands.NewOrders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Endorsements.Queries.GetApprovalsDocumentList
{
    public class GetApprovalDocumentListQuery:IRequest<Response<List<GetApprovalDocumentListDto>>>
    {
        public string OrderId { get; set; }
    }

    public class GetApprovalsDocumentListQueryHandler : IRequestHandler<GetApprovalDocumentListQuery, Response<List<GetApprovalDocumentListDto>>>
    {
        private IApplicationDbContext _context;

        public GetApprovalsDocumentListQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<GetApprovalDocumentListDto>>> Handle(GetApprovalDocumentListQuery request, CancellationToken cancellationToken)
        {
            var list = _context.Documents.Include(x=>x.FormDefinition).ThenInclude(x=>x.FormDefinitionActions).Where(x => x.OrderId == request.OrderId).Select(x => new GetApprovalDocumentListDto
            { Name = x.Name, 
             
                Actions = x.DocumentActions
                .Select(x => new Action { Choice = x.Choice, Title = x.Title,Type=x.Type, State = x.State }).ToList()
            }).ToList();
            return Response<List<GetApprovalDocumentListDto>>.Success(list, 200);
        }
    }
}
