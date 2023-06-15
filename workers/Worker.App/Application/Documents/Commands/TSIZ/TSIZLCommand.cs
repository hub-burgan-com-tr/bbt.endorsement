using Application.Common.Interfaces;
using Application.TsizlFora.Model;
using Application.TsizlLFora;
using Dms.Integration.Infrastructure.Document;
using Dms.Integration.Infrastructure.DocumentServices;
using Dms.Integration.Infrastructure.Models;
using Dms.Integration.Infrastructure.Services;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;

namespace Worker.App.Application.Documents.Commands.CreateDMSDocuments;

public class TSIZLCommand : IRequest<Response<TSIZLResponse>>
{
    public string InstanceId { get; set; }
}

public class TSIZLCommandHandler : IRequestHandler<TSIZLCommand, Response<TSIZLResponse>>
{
    private ITsizlForaService _tsizlForaService = null!;
    private Application.Common.Interfaces.IApplicationDbContext _context;

    public TSIZLCommandHandler(ITsizlForaService tsizlForaService, Application.Common.Interfaces.IApplicationDbContext context)
    {
        _tsizlForaService = tsizlForaService;
        _context = context;
    }


    public async Task<Response<TSIZLResponse>> Handle(TSIZLCommand request, CancellationToken cancellationToken)
    {
        var order = _context.Orders.Include(x => x.Person).Include(x => x.Customer).FirstOrDefault(x => x.OrderId == request.InstanceId.ToString() 
                                                                                                    && x.Title == "Nitelikli Yatırımcı Beyanı - NYB");
        //todo:sistem parametreleri tablosu eklenince burayıda degiştirecez

        if (order == null)
            return Response<TSIZLResponse>.Success(null, 404);

        var customer = order.Customer;
        var dmses = new TSIZLResponse();
        
        if (order.State != OrderState.Approve.ToString())
            return Response<TSIZLResponse>.Success(dmses, 200);
        
        dmses = _tsizlForaService.DoAutomaticEngagementPlain(customer.BranchCode, customer.CustomerNumber.ToString()).Result.Data;
        if (dmses.HasError)
        {
            return Response<TSIZLResponse>.Fail(dmses.ErrorMessage, 417);
        }

        return Response<TSIZLResponse>.Success(dmses, 200);
    }

    
}