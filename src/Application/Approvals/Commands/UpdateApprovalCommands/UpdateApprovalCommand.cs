using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Approvals.Commands.UpdateApprovalCommands;

public class UpdateApprovalCommand : IRequest<Response<bool>>
{
    /// <summary>
    /// Onay Id
    /// </summary>
    public int ApprovalId { get; set; }
    /// <summary>
    /// InstanceId
    /// </summary>
    public string InstanceId { get; set; }
    /// <summary>
    ///Baslik
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// İşlem
    /// </summary>
    public string Process { get; set; }
    /// <summary>
    /// Aşama
    /// </summary>
    public string Stage { get; set; }
    /// <summary>
    /// İşlem No
    /// </summary>
    public string TransactionNumber { get; set; }
    /// <summary>
    /// Geçerlilik
    /// </summary>
    public string TimeoutMinutes { get; set; }
    /// <summary>
    /// Hatırlatma Frekansı
    /// </summary>
    public string RetryFrequence { get; set; }
    /// <summary>
    /// Hatırlatma Sayısı
    /// </summary>
    public int MaxRetryCount { get; set; }
}

public class UpdateApprovalCommandHandler : IRequestHandler<UpdateApprovalCommand, Response<bool>>
{
    private readonly IApplicationDbContext _context;

    public UpdateApprovalCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    /// <summary>
    /// Yeni Onay Emri Guncelleme
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Response<bool>> Handle(UpdateApprovalCommand request, CancellationToken cancellationToken)
    {
        return Response<bool>.Success(data:true, 200);
    }

}

