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
            var response = await _context.Orders.Include(x => x.Documents).ThenInclude(x=>x.Actions).Where(x=>x.OrderId==request.OrderId).Select(x => new GetWantApprovalDetailsDto { OrderId = x.OrderId, Title = x.Title, NameAndSurname ="",Process=x.Reference.Process,State=x.Reference.State,ProcessNo=x.Reference.ProcessNo,MaxRetryCount=x.Config.MaxRetryCount,RetryFrequence=x.Config.RetryFrequence,ExpireInMinutes=x.Config.ExpireInMinutes,History=null,Documents=x.Documents.Select(x=>new GetWantApprovalDocumentDetailsDto { DocumentId=x.DocumentId,Name=x.Name,TypeName= x.Type == "HTML" ? "Metin" : "Belge",Title=x.Actions.FirstOrDefault().Title}).ToList() }).FirstOrDefaultAsync();
                return Response<GetWantApprovalDetailsDto>.Success(response, 200);
        }
    }
}
