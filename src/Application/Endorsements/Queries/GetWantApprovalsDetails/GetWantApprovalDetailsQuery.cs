using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Endorsements.Commands.NewOrders;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Endorsements.Queries.GetWantApprovalsDetails
{
    public class GetWantApprovalDetailsQuery : IRequest<Response<GetWantApprovalDetailsDto>>
    {
        /// <summary>
        /// Onay Id
        /// </summary>
        public long CitizenshipNumber { get; set; }

        public string OrderId { get; set; }
    }

    /// <summary>
    /// İstedigim Onaylar Detay Sayfası
    /// </summary>
    public class GetWantApprovalDetailQueryHandler : IRequestHandler<GetWantApprovalDetailsQuery, Response<GetWantApprovalDetailsDto>>
    {
        private IApplicationDbContext _context;

        public GetWantApprovalDetailQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<GetWantApprovalDetailsDto>> Handle(GetWantApprovalDetailsQuery request, CancellationToken cancellationToken)
        {
            var response = await _context.Orders.Where(x => x.Customer.CitizenshipNumber == request.CitizenshipNumber).Include(x=>x.Customer).Include(x => x.Documents).ThenInclude(x=>x.DocumentActions).Include(x=>x.OrderHistories).Where(x=>x.OrderId==request.OrderId).Select(x => new GetWantApprovalDetailsDto 
            {
                OrderId = x.OrderId,
                Title = x.Title,
                NameAndSurname =x.Customer.FirstName+" "+x.Customer.LastName,
                Process=x.Reference.Process,
                State=x.Reference.State,
                ProcessNo=x.Reference.ProcessNo,
                MaxRetryCount=x.Config.MaxRetryCount,
                RetryFrequence=x.Config.RetryFrequence,
                ExpireInMinutes=x.Config.ExpireInMinutes,
                OrderState=x.State,
                History = x.OrderHistories.OrderByDescending(x=>x.Created).Select(x => new GetWantApprovalDetailsHistoryDto { CreatedDate = x.Created.ToString("dd.MM.yyyy HH:mm"), Description = x.Description, State = x.State }).ToList(),
                Documents =x.Documents.OrderByDescending(x=>x.Created).Select(x=>new GetWantApprovalDocumentDetailsDto { DocumentId=x.DocumentId,Name=x.Name,MimeType=x.MimeType,TypeName= x.Type == ContentType.PlainText.ToString() ? "Metin" : "Belge",Title=x.DocumentActions.FirstOrDefault(y=>y.IsSelected).Title,Content=x.Content,Type=x.FileType}).ToList() }).FirstOrDefaultAsync();
                return Response<GetWantApprovalDetailsDto>.Success(response, 200);
        }
    }
}
