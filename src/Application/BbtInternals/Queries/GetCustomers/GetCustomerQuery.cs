using Application.BbtInternals.Models;
using Application.BbtInternals.Queries.GetSearchPersonSummary;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.BbtInternals.Queries.GetCustomers;

public class GetCustomerQuery : IRequest<Response<GetSearchPersonSummaryResponse>>
{
    public CustomerRequest Customer { get; set; }   
}

public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, Response<GetSearchPersonSummaryResponse>>
{
    private IInternalsService _internalsService;

    public GetCustomerQueryHandler(IInternalsService internalsService)
    {
        _internalsService = internalsService;
    }

    public async Task<Response<GetSearchPersonSummaryResponse>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var response = await _internalsService.GetCustomerSearch(request.Customer);
        if (response.Data == null)
            return Response<GetSearchPersonSummaryResponse>.NotFoundException("Müşteri bulunamadı", 404);

        var persons = response.Data.CustomerList.Select(x => new GetSearchPersonSummaryDto
        {
            CitizenshipNumber = x.CitizenshipNumber,
            First = x.Name.First,
            Last = x.Name.Last,
            ClientNumber = x.CustomerNumber,
            IsPersonel = x.IsPersonel,
            Email = x.Email,
            GsmPhone = x.GsmPhone,
            // Authory = x.IsPersonel == true && x.Authory != null ? new GetSearchPersonSummaryDto.AuthoryModel { IsBranchApproval = x.Authory.IsBranchApproval, IsReadyFormCreator = x.Authory.IsReadyFormCreator, IsNewFormCreator = x.Authory.IsNewFormCreator, IsFormReader = x.Authory.IsFormReader, IsBranchFormReader = x.Authory.IsBranchFormReader } : null,
        });
        return Response<GetSearchPersonSummaryResponse>.Success(new GetSearchPersonSummaryResponse { Persons = persons }, 200);
    }
}