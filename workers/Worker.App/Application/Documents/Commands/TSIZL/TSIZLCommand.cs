

using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;
using Worker.App.Infrastructure.Services;

namespace Worker.App.Application.Documents.Commands.TSIZL;

public class TSIZLCommand : IRequest<Response<TSIZLResponse>>
{
    public string InstanceId { get; set; }
}

public class TSIZLCommandHandler : IRequestHandler<TSIZLCommand, Response<TSIZLResponse>>
{
    private ITsizlForaService _tsizlForaService = null!;

    private IApplicationDbContext _context;

    public TSIZLCommandHandler(ITsizlForaService tsizlForaService, IApplicationDbContext context)
    {
        _tsizlForaService = tsizlForaService;
        _context = context;
    }


    public async Task<Response<TSIZLResponse>> Handle(TSIZLCommand request, CancellationToken cancellationToken)
    {
        var order = _context.Orders.Include(x => x.Person).Include(x => x.Customer).FirstOrDefault(x => x.OrderId == request.InstanceId.ToString() 
                                    && (x.Title == "Nitelikli Yatırımcı Beyanı - NYB" || x.Title == "Hesap Açılış Sözleşmesi - Fonlu Mevduat"));
        //todo:sistem parametreleri tablosu eklenince burayıda degiştirecez

        if (order == null)
            return Response<TSIZLResponse>.Success(null, 404);

        var customer = order.Customer;
        var TSIZLResponse = new TSIZLResponse();

        if (order.State != OrderState.Approve.ToString())
            return Response<TSIZLResponse>.Success(TSIZLResponse, 200);

        try
        {
            string engagementKind = order.Title == "Nitelikli Yatırımcı Beyanı - NYB" ? "S9" : order.Title == "Hesap Açılış Sözleşmesi - Fonlu Mevduat" ? "FM" : "";
            var serviceTSIZLResponse = await _tsizlForaService.DoAutomaticEngagementPlain(customer.BranchCode, customer.CustomerNumber.ToString(), engagementKind);
            if (serviceTSIZLResponse != null)
            {

                TSIZLResponse.ErrorMessage = serviceTSIZLResponse.ErrorMessage;
                TSIZLResponse.ReferenceNumber = serviceTSIZLResponse.ReferenceNumber;
                TSIZLResponse.HasError = serviceTSIZLResponse.HasError;
            }

            if (TSIZLResponse.HasError)
            {
                return Response<TSIZLResponse>.Fail(TSIZLResponse.ErrorMessage, 417);
            }
        }
        catch (Exception ex)
        {
            Log.ForContext("serviceTSIZLResponse OrderId", request.InstanceId).Error(ex, ex.Message);
        }


        return Response<TSIZLResponse>.Success(TSIZLResponse, 200);
    }


}