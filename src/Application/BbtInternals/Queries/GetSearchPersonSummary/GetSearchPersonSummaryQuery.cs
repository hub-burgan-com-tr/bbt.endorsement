﻿using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Models;
using MediatR;

namespace Application.BbtInternals.Queries.GetSearchPersonSummary
{
    public class GetSearchPersonSummaryQuery : IRequest<Response<GetSearchPersonSummaryResponse>>
    {
        public string Name { get; set; }
        public OrderPerson Person { get; set; }
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
            if (response.Data == null)
                return Response<GetSearchPersonSummaryResponse>.Fail("Pesponse.Data NULL", 201);
            if (response.Data.CustomerList == null)
                return Response<GetSearchPersonSummaryResponse>.Fail("Pesponse.Data.CustomerList NULL", 201);
            if (!response.Data.CustomerList.Any(x => x.RecordStatus == "A"))
                return Response<GetSearchPersonSummaryResponse>.NotFoundException("Müşteri bulunamadı", 404);
            
            var persons = response.Data.CustomerList.Where(x => x.RecordStatus == "A").Select(x => new GetSearchPersonSummaryDto
            {
                CitizenshipNumber = x.CitizenshipNumber,
                First = x.Name.First,
                Last = x.Name.Last,
                CustomerNumber = x.CustomerNumber,
                IsStaff = x.IsStaff,
                Email = x.Email,
                TaxNo=x.TaxNo,
                GsmPhone = x.GsmPhone,
                Authory = x.IsStaff == true && x.Authory != null ? new AuthoryModel { IsBranchApproval = x.Authory.IsBranchApproval, IsReadyFormCreator = x.Authory.IsReadyFormCreator, IsNewFormCreator = x.Authory.IsNewFormCreator, IsFormReader = x.Authory.IsFormReader, IsBranchFormReader = x.Authory.IsBranchFormReader, isUIVisible = x.Authory.isUIVisible } : null,
            });
            persons = persons.OrderBy(x => x.CustomerNumber);

            if (request.Person.IsBranchApproval == false)
                persons = persons.Where(x => x.BranchCode == request.Person.BranchCode);

            return Response<GetSearchPersonSummaryResponse>.Success(new GetSearchPersonSummaryResponse { Persons = persons.OrderBy(x => x.CustomerNumber) }, 200);
        }
    }
}