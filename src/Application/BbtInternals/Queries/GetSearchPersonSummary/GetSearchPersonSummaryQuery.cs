using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.BbtInternals.Queries.GetSearchPersonSummary
{
    public class GetSearchPersonSummaryQuery : IRequest<Response<GetSearchPersonSummaryResponse>>
    {
        public string Name { get; set; }
    }

    public class GetSearchPersonSummaryQueryHandler : IRequestHandler<GetSearchPersonSummaryQuery, Response<GetSearchPersonSummaryResponse>>
    {
        private IInternalsService _internalsService;

        public GetSearchPersonSummaryQueryHandler(IInternalsService internalsService)
        {
            _internalsService = internalsService;
        }

        public async Task<Response<GetSearchPersonSummaryResponse>> Handle(GetSearchPersonSummaryQuery request, CancellationToken cancellationToken)
        {
            var response = await _internalsService.GetCustomerSearchByName(new Models.CustomerSearchRequest { name = request.Name , page = 1, size = 10});
            var persons = response.Data.CustomerList.Select(x => new GetSearchPersonSummaryDto
            {
                CitizenshipNumber = x.CitizenshipNumber,
                First = x.Name.First,
                Last = x.Name.Last,
                CustomerNumber = x.CustomerNumber,
                IsStaff = x.IsStaff,
                Email = x.Email,
                TaxNo=x.TaxNo,
             //   GsmPhones = x.GsmPhones,
                Authory = x.IsStaff == false && x.Authory != null ? new AuthoryModel { IsBranchApproval = x.Authory.IsBranchApproval, IsReadyFormCreator = x.Authory.IsReadyFormCreator, IsNewFormCreator = x.Authory.IsNewFormCreator, IsFormReader = x.Authory.IsFormReader, IsBranchFormReader = x.Authory.IsBranchFormReader } : null,
            });
            return Response<GetSearchPersonSummaryResponse>.Success(new GetSearchPersonSummaryResponse { Persons = persons }, 200);
        }
    }
}