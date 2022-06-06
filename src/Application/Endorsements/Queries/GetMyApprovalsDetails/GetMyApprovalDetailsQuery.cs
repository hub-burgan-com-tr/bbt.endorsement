using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Endorsements.Commands.NewOrders;
using Application.Services;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Application.Endorsements.Queries.GetMyApprovalsDetails
{
    public class GetMyApprovalDetailsQuery : IRequest<Response<GetMyApprovalDetailsDto>>
    {/// <summary>
     /// Onay Id
     /// </summary>
        public string OrderId { get; set; }
        public long CitizenshipNumber { get; set; }

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
            var response = _context.Orders.Where(x => x.State != OrderState.Pending.ToString() && x.State != OrderState.Cancel.ToString()&&x.Customer.CitizenshipNumber==request.CitizenshipNumber).Include(x => x.Documents).ThenInclude(x => x.DocumentActions).Include(x => x.Documents).ThenInclude(x => x.FormDefinition).ThenInclude(x => x.FormDefinitionActions).Include(x=>x.OrderHistories).Where(x => x.OrderId == request.OrderId)
                .Select(x => new GetMyApprovalDetailsDto
                {
                    Title = x.Title,
                    Documents=x.Documents.OrderByDescending(x=>x.Created).Select(x=>new OrderDocument { Name=x.Name,
                    Content=x.Type==ContentType.PlainText.ToString()? DecodeBase64Services.DecodeBase64(x.Content): x.Content,
                    OrderState=x.Order.State,
                    MimeType=x.MimeType,
                    State=x.State== ActionType.Approve.ToString()?true:false,
                    Type =x.FileType,                        
                       Actions=x.DocumentActions.Where(x=>x.IsSelected).Select(y=>new Action { Checked = y.IsSelected, Title = y.Title, DocumentId = x.DocumentId }).FirstOrDefault()}).ToList(),
                    History = x.OrderHistories.Where(x=>x.IsStaff).OrderByDescending(x=>x.Created).Select(x => new GetMyApprovalDetailHistoryDto { CreatedDate = x.Created.ToString("dd.MM.yyyy HH:mm"), Description = x.Description, State = x.State }).ToList()

                }).ToList();

            return Response<GetMyApprovalDetailsDto>.Success(response.FirstOrDefault(), 200);
        }
       
    }
}
