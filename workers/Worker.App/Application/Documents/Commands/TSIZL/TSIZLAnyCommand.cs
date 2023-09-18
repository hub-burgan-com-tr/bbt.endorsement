using Worker.App.Application.Common.Interfaces;
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

namespace Worker.App.Application.Documents.Commands.TSIZL;

public class TSIZLAnyCommand : IRequest<Response<bool>>
{
    public string InstanceId { get; set; }
}

public class TSIZLAnyCommandHandler : IRequestHandler<TSIZLAnyCommand, Response<bool>>
{
    private IApplicationDbContext _context;

    public TSIZLAnyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<Response<bool>> Handle(TSIZLAnyCommand request, CancellationToken cancellationToken)
    {
        var order = _context.Orders.Any(x => x.OrderId == request.InstanceId.ToString() 
                                    && (x.Title == "Nitelikli Yatırımcı Beyanı - NYB" || x.Title == "Hesap Açılış Sözleşmesi - Fonlu Mevduat"));
        //todo:sistem parametreleri tablosu eklenince burayıda degiştirecez

        return Response<bool>.Success(order, 200);
    }
}