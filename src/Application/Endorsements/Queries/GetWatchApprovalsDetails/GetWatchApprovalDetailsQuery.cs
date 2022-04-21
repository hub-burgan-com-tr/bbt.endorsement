using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Endorsements.Commands.NewOrders;
using Domain.Enums;
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
            var response = await _context.Orders.Where(x => x.State != OrderState.Cancel.ToString()).Include(x=>x.Customer).Include(x=>x.OrderHistories).Include(x => x.Documents).ThenInclude(x => x.DocumentActions).Where(x => x.OrderId == request.OrderId).Select(x => new GetWatchApprovalDetailsDto 
            { 
                OrderId = x.OrderId, 
                Title = x.Title,
                NameAndSurname = x.Customer.FirstName+" "+x.Customer.LastName,
                Process = x.Reference.Process,
                State = x.Reference.State,
                ProcessNo = x.Reference.ProcessNo,
                MaxRetryCount = x.Config.MaxRetryCount,
                RetryFrequence = x.Config.RetryFrequence,
                ExpireInMinutes = x.Config.ExpireInMinutes,
                OrderState=x.State,
                History = x.OrderHistories.OrderByDescending(x=>x.Created).Select(x => new GetWatchApprovalDetailsHistoryDto { CreatedDate = x.Created.ToString("dd.MM.yyyy HH:mm"), Description = x.Description, State = x.State }).ToList(),
                Documents = x.Documents.OrderByDescending(x=>x.Created).Select(x => new GetWatchApprovalDocumentDetailsDto { DocumentId = x.DocumentId, Name = x.Name,MimeType=x.MimeType, TypeName = x.Type ==ContentType.PlainText.ToString() ? "Metin" : "Belge", Title = x.DocumentActions.FirstOrDefault(y => y.IsSelected).Title, Content = x.Content, Type = x.FileType }).ToList() }).FirstOrDefaultAsync();
            return Response<GetWatchApprovalDetailsDto>.Success(response, 200);
        }
    }
}
