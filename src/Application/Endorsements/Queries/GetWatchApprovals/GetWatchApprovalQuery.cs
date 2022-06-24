using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Mappings;
using Application.Endorsements.Commands.NewOrders;
using Domain.Enums;

namespace Application.Endorsements.Queries.GetWatchApprovals
{
    public class GetWatchApprovalQuery : IRequest<Response<PaginatedList<GetWatchApprovalDto>>>
    {
        /// <summary>
        /// Onay Isteyen
        /// </summary>
        public string Approver { get; set; }
        /// <summary>
        /// Onaycı
        /// </summary>
        public string Customer { get; set; }
        public string Process { get; set; }
        public string State { get; set; }
        public string ProcessNo { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

    }

    /// <summary>
    /// İzleme Listesi
    /// </summary>
    public class GetWatchApprovalQueryHandler : IRequestHandler<GetWatchApprovalQuery, Response<PaginatedList<GetWatchApprovalDto>>>
    {
        private IApplicationDbContext _context;
        public GetWatchApprovalQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<PaginatedList<GetWatchApprovalDto>>> Handle(GetWatchApprovalQuery request, CancellationToken cancellationToken)
        {


            IQueryable<Order> orders = _context.Orders.OrderByDescending(x => x.Created).Include(x => x.Documents).Include(x => x.Person).Include(x => x.Customer);
            if (!string.IsNullOrEmpty(request.Customer))
                orders = orders.Where(x => x.Customer.FirstName.Contains(request.Customer.Trim())
                                                     || x.Customer.LastName.Contains(request.Customer.Trim()) ||
                                                     (x.Customer.FirstName + " " + x.Customer.LastName).Contains(request.Customer.Trim())||x.Customer.CustomerNumber.ToString()==request.Customer.ToString()||x.Customer.CitizenshipNumber.ToString()== request.Customer.ToString());
            if (!string.IsNullOrEmpty(request.Approver))
                orders = orders.Where(x => x.Person.FirstName.Contains(request.Approver.Trim())
                                                                    || x.Person.LastName.Contains(request.Approver.Trim()) ||
                                                                    (x.Person.FirstName + " " + x.Person.LastName).Contains(request.Approver.Trim())||x.Person.CitizenshipNumber.ToString()==request.Approver.ToString());
            if (!string.IsNullOrEmpty(request.Process))
                orders = orders.Where(x => x.Reference.Process.Contains(request.Process.Trim()));
            if (!string.IsNullOrEmpty(request.State))
                orders = orders.Where(x => x.Reference.State.Contains(request.State.Trim()));
            if (!string.IsNullOrEmpty(request.ProcessNo))
                orders = orders.Where(x => x.Reference.ProcessNo == request.ProcessNo.Trim());
            var list = await orders.OrderByDescending(x=>x.Created)
              .Select(x => new GetWatchApprovalDto
              {
                  OrderId = x.OrderId,
                  Title = x.Title,
                  Customer = x.Customer.FirstName+" "+x.Customer.LastName,
                  Approver = x.Person.FirstName+" "+x.Person.LastName,
                  Process = x.Reference.Process,
                  State = x.Reference.State,
                  ProcessNo = x.Reference.ProcessNo,
                  Date = x.Created.ToString("dd.MM.yyyy HH:mm"),
                  IsDocument = x.Documents.Any(x => x.Type != ContentType.PlainText.ToString()),
                  OrderState = x.State,
              }).PaginatedListAsync(request.PageNumber, request.PageSize);

            return Response<PaginatedList<GetWatchApprovalDto>>.Success(list, 200);
        }
    }
}