using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
                    
                    Documents = x.Documents.Where(x=>x.State==null).Select(y=>new OrderDocument
                    {
                        Content=y.Content,
                        Link=y.Name,
                        Name=y.Name,
                        Type=y.Type,                     
                       DocumentId = y.DocumentId,
                        Actions=y.DocumentActions.OrderByDescending(x=>x.Created).Select(z=>new DocumentAction { DocumentActionId=z.DocumentActionId, Value = z.DocumentActionId, Title=z.Title}).ToList()
                    }).ToList(),                
                   }).FirstOrDefault();
          
            return Response<GetApprovalDetailsDto>.Success(response, 200);
        }
    }
}
