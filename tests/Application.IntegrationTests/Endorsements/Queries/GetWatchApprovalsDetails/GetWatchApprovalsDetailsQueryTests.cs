using System.Threading.Tasks;
using Application.Endorsements.Queries.GetWatchApprovalsDetails;
using Application.IntegrationTests.Services;
using NUnit.Framework;
using static Application.IntegrationTests.Testing;
namespace Application.IntegrationTests.Endorsements.Queries.GetWatchApprovalsDetails
{
    public class GetWatchApprovalsDetailsQueryTests:TestBase
    {

        [TestCaseSource(typeof(EndorsementService), nameof(EndorsementService.GetWatchApprovalsDetailsQueryData))]

        public async Task GetWatchApprovalsDetailsQueryTestAsync(string orderId)
            {
                var response = await SendAsync(new GetWatchApprovalDetailsQuery { OrderId = orderId });
                //Assert.IsNotNull(response);
            }
    }
}
