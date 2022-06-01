using Application.Common.Interfaces;
using Application.Endorsements.Commands.ApproveOrderDocuments;
using Application.Endorsements.Commands.CancelOrders;
using Application.Endorsements.Queries.GetWatchApprovals;
using Domain.Enums;
using Domain.Models;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Configuration;
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


    public static StartRequest NewOrderTestData()
    {
        var document =new List<OrderDocument>() { new OrderDocument {Type=2,Title="Metin",Content="İçerik",Actions=new List<DocumentActionClass>() { new DocumentActionClass { Choice=1,Title= "Okudum, onayladım" } } } };
        var StartRequest = new StartRequest
        {
            Approver = new OrderCustomer { CitizenshipNumber= 56906430700, First= "Ahmet",Last= "Güler",CustomerNumber= 130991279 },
            Documents=document,
            Title= "Sigorta Teklif Formu",
            Reference = new OrderReference { Callback = null, Process = "123", ProcessNo = "Sigorta Formları", State = "Başvuru Formu" },
            Config = new OrderConfig { ExpireInMinutes = 60, MaxRetryCount = 3, RetryFrequence = 15 },
        };
        return StartRequest;
    }



    public static CancelOrderCommand CancelOrderTestData()
    {
        IApplicationDbContext _context = new ApplicationDbContext(null, null, null);
        var order = _context.Orders.FirstOrDefault(x => x.State != OrderState.Cancel.ToString());
        return new CancelOrderCommand { orderId = order == null ? "" : order.OrderId };
    }

    public static ApproveOrderDocumentCommand ApproveOrderDocumentTestData()
    {
        string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        IApplicationDbContext _context = new ApplicationDbContext(optionsBuilder.Options, null, null);


        var order = _context.Orders.Include(x => x.Documents).ThenInclude(x => x.DocumentActions).Include(x => x.Documents).ThenInclude(x => x.FormDefinition).ThenInclude(x => x.FormDefinitionActions).FirstOrDefault(x => x.State != OrderState.Cancel.ToString());
        ApproveOrderDocumentCommand approveOrderDocument = new ApproveOrderDocumentCommand
        {
            OrderId = order?.OrderId,
            Documents=new List<ApproveOrderDocument> { new ApproveOrderDocument { DocumentId=order?.Documents?.FirstOrDefault()?.DocumentId,ActionId=order?.Documents.FirstOrDefault()?.FormDefinition?.FormDefinitionActions?.FirstOrDefault()?.FormDefinitionActionId} }

        };
        return approveOrderDocument;
    }

    public static StartFormRequest CreateOrUpdateFormTestData()
    {
     StartFormRequest request=new StartFormRequest
     {
         Approver=new OrderCustomer { CitizenshipNumber= 17556080776 ,First= "Merve",Last= "Aydın",CustomerNumber= 514250868 },
         Content= "{\"textField\":\"                              SIGORTA BASVURU FORMU\",\"sigortaliAdiUnvani\":\"AHMET \",\"sigortaliVknTckn\":\"29525155454\",\"sigortaEttirenAdiUnvani\":\"MEHMET\",\"sigortaEttirenVknTckn\":\"29525133478\",\"dainiMurtehin\":\"Selim\",\"sigortaTuru\":{\"esya\":true,\"konut\":false,\"dask\":false,\"kasko\":false,\"trafik\":false,\"isyeri\":false,\"diger\":false},\"textArea\":\"Kredi teminatı olan sigortalarda, sigorta bedeli, sigortanın konusunun piyasa değeri ve değerleme raporundaki minimum sigorta bedelini karşılar. Poliçede dain-i mürtehin Burgan Bank A.Ş. olur. Sigorta ettiren, sigorta şirketini seçmekte serbesttir.\",\"textArea1\":\"Yukarıda beyan etmiş olduğum bilgiler çerçevesinde sigorta talebimin alınmasını ve tarafıma sigorta teklifinin yapılmasını talep ederim.\",\"textArea2\":\"Sigorta poliçesi talebiniz kapsamında paylaştığınız kişisel verilerinize ilişkin 6698 sayılı Kişisel Verilerin Korunması Kanunu hakkındaki detaylı bilgilendirmeye www.burgan.com.tr adresinden ulaşabilirsiniz.\",\"adresKoduUavtKodu\":\"295479\",\"binaM2\":\"100\",\"binaSigortaDegeri\":\"1000000\",\"esyaSigortaDegeri\":\"500000\",\"binaYapimYili\":\"2015\",\"toplamKatAdedi\":\"8\",\"bulunduguKat\":\"4\",\"rizikoAdresi\":\"Mahmutbey\",\"ekteIletilenMevcutPoliceSartlarimaGoreDuzenlensin1\":true}",
         FormId= "b25635e8-1abd-4768-ab97-e1285999a61a",
         Reference=new OrderReference {ProcessNo="123456"},
         
         Title= "Sigorta Başvuru Formu",
        
     };
        return request;
    }
}

