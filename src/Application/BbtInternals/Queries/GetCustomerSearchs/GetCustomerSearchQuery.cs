using Application.BbtInternals.Models;
using Application.BbtInternals.Queries.GetSearchPersonSummary;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Models;
using MediatR;

namespace Application.BbtInternals.Queries.GetCustomerSearchs;

public class GetCustomerSearchQuery : IRequest<Response<GetSearchPersonSummaryResponse>>
{
    public string Name { get; set; }
    public OrderPerson Person { get; set; }
}

public class GetCustomerSearchQueryHandler : IRequestHandler<GetCustomerSearchQuery, Response<GetSearchPersonSummaryResponse>>
{
    private IInternalsService _internalsService;

    public GetCustomerSearchQueryHandler(IInternalsService internalsService)
    {
        _internalsService = internalsService;
    }

    public async Task<Response<GetSearchPersonSummaryResponse>> Handle(GetCustomerSearchQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Name))
                return Response<GetSearchPersonSummaryResponse>.NotFoundException("", 404);

            var response = await _internalsService.GetCustomerSearchByName(new CustomerSearchRequest
            {
                name = request.Name,
                page = 1,
                size = 10
            });

            if(response.Data == null)
                return Response<GetSearchPersonSummaryResponse>.Fail("Pesponse.Data NULL", 201);
            if (response.Data.CustomerList == null)
                return Response<GetSearchPersonSummaryResponse>.Fail("Pesponse.Data.CustomerList NULL", 201);


            var persons = response.Data.CustomerList.Select(x => new GetSearchPersonSummaryDto
            {
                CitizenshipNumber = x.CitizenshipNumber,
                First = x.Name.First,
                Last = x.Name.Last,
                CustomerNumber = x.CustomerNumber,
                IsStaff = x.IsStaff,
                Email = x.Email,
                TaxNo=x.TaxNo,
                GsmPhone = x.GsmPhone,
                BranchCode = x.BranchCode,
                BusinessLine=x.BusinessLine,
                // Authory = x.IsPersonel == true && x.Authory != null ? new GetSearchPersonSummaryDto.AuthoryModel { IsBranchApproval = x.Authory.IsBranchApproval, IsReadyFormCreator = x.Authory.IsReadyFormCreator, IsNewFormCreator = x.Authory.IsNewFormCreator, IsFormReader = x.Authory.IsFormReader, IsBranchFormReader = x.Authory.IsBranchFormReader } : null,
            });

            if (!request.Person.IsBranchApproval)
            {
                persons = persons.Where(x => x.BranchCode == request.Person.BranchCode);
            }

            return Response<GetSearchPersonSummaryResponse>.Success(new GetSearchPersonSummaryResponse { Persons = persons }, 200);
        }
        catch (Exception ex)
        {
            return Response<GetSearchPersonSummaryResponse>.Fail(ex.ToString(), 201);
        }
    }
}
