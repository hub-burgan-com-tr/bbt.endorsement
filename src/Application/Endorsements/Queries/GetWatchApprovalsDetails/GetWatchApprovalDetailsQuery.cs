using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Endorsements.Queries.GetWatchApprovalsDetails
{
    public class GetWatchApprovalDetailsQuery : IRequest<Response<GetWatchApprovalDetailsDto>>
    {
    /// <summary>
    /// Onay Id
    /// </summary>
        public string OrderId { get; set; }
    }

    /// <summary>
    /// İzleme Detay Sayfasi
    /// </summary>
    public class GetWatchApprovalDetailsQueryHandler : IRequestHandler<GetWatchApprovalDetailsQuery, Response<GetWatchApprovalDetailsDto>>
    {
        private IApplicationDbContext _context;

        public GetWatchApprovalDetailsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<GetWatchApprovalDetailsDto>> Handle(GetWatchApprovalDetailsQuery request, CancellationToken cancellationToken)
        {
            var response = await _context.Orders.Include(x => x.Documents).ThenInclude(x => x.DocumentActions).Where(x => x.OrderId == request.OrderId).Select(x => new GetWatchApprovalDetailsDto { OrderId = x.OrderId, Title = x.Title, NameAndSurname = "", Process = x.Reference.Process, State = x.Reference.State, ProcessNo = x.Reference.ProcessNo, MaxRetryCount = x.Config.MaxRetryCount, RetryFrequence = x.Config.RetryFrequence, ExpireInMinutes = x.Config.ExpireInMinutes, History = null, Documents = x.Documents.Select(x => new GetWatchApprovalDocumentDetailsDto { DocumentId = x.DocumentId, Name = x.Name, TypeName = x.Type == "HTML" ? "Metin" : "Belge", Title = x.DocumentActions.FirstOrDefault().Title }).ToList() }).FirstOrDefaultAsync();
            return Response<GetWatchApprovalDetailsDto>.Success(response, 200);
        }
    }
}
