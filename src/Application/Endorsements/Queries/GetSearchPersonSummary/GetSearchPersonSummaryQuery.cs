using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Endorsements.Commands.NewOrders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RestSharp;

namespace Application.Endorsements.Queries.GetSearchPersonSummary
{
    public class GetSearchPersonSummaryQuery :IRequest<Response<GetSearchPersonSummaryDto>>
    {
        public string Name { get; set; }

    }

    public class GetSearchPersonSummaryQueryHandler : IRequestHandler<GetSearchPersonSummaryQuery, Response<GetSearchPersonSummaryDto>>
    {
     

        public async Task<Response<GetSearchPersonSummaryDto>> Handle(GetSearchPersonSummaryQuery request, CancellationToken cancellationToken)
        {

            var restClient = new RestClient("http://20.31.226.131:5000");
            var restRequest = new RestRequest("/Person", Method.Get);
            restRequest.AddParameter("name", request.Name);
            var response = restClient.ExecuteAsync<GetSearchPersonSummaryDto>(restRequest);
            return Response<GetSearchPersonSummaryDto>.Success(new GetSearchPersonSummaryDto { CitizenshipNumber = response.Result.Data.CitizenshipNumber, First = response.Result.Data.First, Last = response.Result.Data.Last }, 200);
        }
    }

}
