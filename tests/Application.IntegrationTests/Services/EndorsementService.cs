using Application.Common.Interfaces;
using Application.Endorsements.Queries.GetWatchApprovals;
using Domain.Enums;
using Infrastructure.Persistence;
using System.Linq;

namespace Application.IntegrationTests.Services;

public static class EndorsementService
{
    //private readonly IApplicationDbContext _context;

    //public EndorsementService(IApplicationDbContext context)
    //{
    //    _context = context;
    //}

    public static string GetApprovalQueryData()
    {
        IApplicationDbContext _context = new ApplicationDbContext(null,null,null);
        var order = _context.Orders.FirstOrDefault(x => x.State == OrderState.Pending.ToString());
        return order == null ? "" : order.OrderId;
    }

    public static string GetApprovalDetailQueryData()
    {
        IApplicationDbContext _context = new ApplicationDbContext(null, null, null);
        var order = _context.Orders.FirstOrDefault(x => x.State == OrderState.Pending.ToString());
        return order == null ? "" : order.OrderId;
    }

    public static string GetMyApprovalQueryData()
    {
        IApplicationDbContext _context = new ApplicationDbContext(null, null, null);
        var order = _context.Orders.FirstOrDefault(x => x.State != OrderState.Pending.ToString() && x.State != OrderState.Cancel.ToString());
        return order == null ? "" : order.OrderId;
    }
    public static string GetMyApprovalQueryDetailData()
    {
        IApplicationDbContext _context = new ApplicationDbContext(null, null, null);
        var order = _context.Orders.FirstOrDefault(x => x.State != OrderState.Pending.ToString() && x.State != OrderState.Cancel.ToString());
        return order == null ? "" : order.OrderId;
    }
    public static string GetWantApprovalQueryData()
    {
        IApplicationDbContext _context = new ApplicationDbContext(null, null, null);
        var order = _context.Orders.FirstOrDefault(x => x.State != OrderState.Cancel.ToString());
        return order == null ? "" : order.OrderId;
    }
    public static string GetWantApprovalsDetailData()
    {
        IApplicationDbContext _context = new ApplicationDbContext(null, null, null);
        var order = _context.Orders.FirstOrDefault(x => x.State != OrderState.Cancel.ToString());
        return order == null ? "" : order.OrderId;
    }
    public static GetWatchApprovalQuery GetWatchApprovalQueryData()
    {
        var command = new GetWatchApprovalQuery
        {
            Approver="Ahmet",

        };
        return command;
    }
    public static string GetWatchApprovalsDetailsQueryData()
    {
        IApplicationDbContext _context = new ApplicationDbContext(null, null, null);
        var order = _context.Orders.FirstOrDefault(x => x.State != OrderState.Cancel.ToString());
        return order == null ? "" : order.OrderId;
    }
}

