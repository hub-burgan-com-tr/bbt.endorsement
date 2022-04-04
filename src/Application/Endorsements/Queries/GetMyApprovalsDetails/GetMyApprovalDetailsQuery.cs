using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Endorsements.Commands.NewOrders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Endorsements.Queries.GetMyApprovalsDetails
{
    public class GetMyApprovalDetailsQuery : IRequest<Response<GetMyApprovalDetailsDto>>
    {/// <summary>
     /// Onay Id
     /// </summary>
        public string OrderId { get; set; }

    }
    /// <summary>
    /// Onayladıklarım Detay Sayfası
    /// </summary>
    public class GetMyApprovalDetailQueryHandler : IRequestHandler<GetMyApprovalDetailsQuery, Response<GetMyApprovalDetailsDto>>
    {
        private IApplicationDbContext _context;

        public GetMyApprovalDetailQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<GetMyApprovalDetailsDto>> Handle(GetMyApprovalDetailsQuery request, CancellationToken cancellationToken)
        {
            var response = _context.Orders.Include(x => x.Documents).ThenInclude(x => x.DocumentActions).Include(x => x.Documents).ThenInclude(x => x.FormDefinition).ThenInclude(x => x.FormDefinitionActions).Include(x=>x.OrderHistories).Where(x => x.OrderId == request.OrderId)
                .Select(x => new GetMyApprovalDetailsDto
                {
                    Title = x.Title,
                    Documents=x.Documents.Select(x=>new OrderDocument { Name=x.Name,
                        
                        Actions=x.FormDefinitionId!=null?x.FormDefinition.FormDefinitionActions.Select(y=>new Action { Checked = y.State==ActionType.Approve.ToString()?true:false, Title = y.Title, DocumentId = x.DocumentId }).ToList():x.DocumentActions.Select(y=>new Action { Checked = x.State == ActionType.Approve.ToString() ? true : false, Title = y.Title, DocumentId = x.DocumentId }).ToList()}).ToList(),
                    History = x.OrderHistories.Select(x => new GetMyApprovalDetailHistoryDto { CreatedDate = x.Created.ToString("dd.MM.yyyy HH:mm"), Description = x.Description, State = x.State }).ToList()

                }).FirstOrDefault();
            return Response<GetMyApprovalDetailsDto>.Success(response, 200);
        }
    }
}
