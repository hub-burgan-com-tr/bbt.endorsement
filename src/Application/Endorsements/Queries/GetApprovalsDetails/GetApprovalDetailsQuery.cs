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
            var response = _context.Orders.Include(x => x.Documents).ThenInclude(x => x.DocumentActions).Include(x => x.Customer).Include(x=>x.Documents).ThenInclude(x=>x.FormDefinition).ThenInclude(x=>x.FormDefinitionActions).Where(x => x.OrderId == request.OrderId)
                .Select(x => new GetApprovalDetailsDto
                { 
                    Title = x.Title,
                    CitizenShipNumber = x.Customer.CitizenshipNumber, 
                    FirstAndSurname = x.Customer.FirstName + " " + x.Customer.LastName, 
                    
                    Documents = x.Documents.Select(y=>new OrderDocument
                    {
                        Content=y.Content,
                        Link=y.Name,
                        Name=y.Name,
                        Choice = y.FormDefinitionId != null ? y.FormDefinition.FormDefinitionActions.FirstOrDefault().IsDefault ? (int)DocumentApprovedEnum.Approved : (int)DocumentApprovedEnum.Rejected : y.DocumentActions.FirstOrDefault().IsDefault ? (int)DocumentApprovedEnum.Approved : (int)DocumentApprovedEnum.Rejected,
                       DocumentId = y.DocumentId,
                        Actions=y.FormDefinitionId!=null?y.FormDefinition.FormDefinitionActions.Select(z=>new DocumentAction {Value=z.IsDefault?(int)DocumentApprovedEnum.Approved:(int)DocumentApprovedEnum.Rejected,DocumentActionId=z.FormDefinitionActionId,Title=z.Title }).ToList():y.DocumentActions.Select(z=>new DocumentAction { DocumentActionId=z.DocumentActionId, Value = z.IsDefault ? (int)DocumentApprovedEnum.Approved : (int)DocumentApprovedEnum.Rejected, Title=z.Title}).ToList()
                    }).ToList(),                
                   }).FirstOrDefault();
          
            return Response<GetApprovalDetailsDto>.Success(response, 200);
        }
    }
}
