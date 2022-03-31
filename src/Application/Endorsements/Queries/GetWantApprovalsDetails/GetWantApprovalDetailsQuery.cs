using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Endorsements.Queries.GetWantApprovalsDetails
{
    public class GetWantApprovalDetailsQuery : IRequest<Response<GetWantApprovalDetailsDto>>
    {
        /// <summary>
        /// Onay Id
        /// </summary>
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
            var response = await _context.Orders.Include(x=>x.Customer).Include(x => x.Documents).ThenInclude(x=>x.DocumentActions).Include(x=>x.OrderHistories).Where(x=>x.OrderId==request.OrderId).Select(x => new GetWantApprovalDetailsDto 
            { OrderId = x.OrderId,
                Title = x.Title,
                NameAndSurname =x.Customer.FirstName+" "+x.Customer.LastName,
                Process=x.Reference.Process,
                State=x.Reference.State,
                ProcessNo=x.Reference.ProcessNo,
                MaxRetryCount=x.Config.MaxRetryCount,
                RetryFrequence=x.Config.RetryFrequence,
                ExpireInMinutes=x.Config.ExpireInMinutes,
                History = x.OrderHistories.Select(x => new GetWantApprovalDetailsHistoryDto { CreatedDate = DateTime.Now.ToString("dd MM yyyy HH:mm"), Name = x.Name, State = x.State }).ToList(),
                Documents =x.Documents.Select(x=>new GetWantApprovalDocumentDetailsDto { DocumentId=x.DocumentId,Name=x.Name,TypeName= x.Type == "HTML" ? "Metin" : "Belge",Title=x.DocumentActions.FirstOrDefault().Title}).ToList() }).FirstOrDefaultAsync();
                return Response<GetWantApprovalDetailsDto>.Success(response, 200);
        }
    }
}
