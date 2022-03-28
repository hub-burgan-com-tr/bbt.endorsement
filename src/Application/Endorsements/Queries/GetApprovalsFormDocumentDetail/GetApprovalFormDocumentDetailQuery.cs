using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Endorsements.Commands.NewOrders;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            var response = _context.Documents.Include(x=>x.Order).ThenInclude(x=>x.Customer).Include(x=>x.FormDefinition).ThenInclude(x=>x.FormDefinitionActions).Where(x => x.OrderId == request.OrderId && x.Type == ContentType.HTML.ToString()).Select(x => new GetApprovalFormDocumentDetailDto {DocumentId=x.DocumentId,Title=x.Order.Title, Name = x.Name, CitizenShipNumber = x.Order.Customer.CitizenshipNumber, FirstAndSurname = x.Order.Customer.FirstName+" "+x.Order.Customer.LastName,Content=x.Content, Actions = x.FormDefinition.FormDefinitionActions.Select(x => new Action {ActionId=x.FormDefinitionActionId, IsDefault = x.IsDefault, Title = x.Title }).ToList() }).FirstOrDefault(); 
            return Response<GetApprovalFormDocumentDetailDto>.Success(response, 200);
        }
    }
}
