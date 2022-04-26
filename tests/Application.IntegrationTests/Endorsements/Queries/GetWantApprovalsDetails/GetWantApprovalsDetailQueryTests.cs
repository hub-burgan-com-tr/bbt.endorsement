using System.Threading.Tasks;
using Application.Endorsements.Queries.GetWantApprovalsDetails;
using Application.IntegrationTests.Services;
using NUnit.Framework;
using static Application.IntegrationTests.Testing;

namespace Application.IntegrationTests.Endorsements.Queries.GetWantApprovalsDetails
{
    public class GetWantApprovalsDetailQueryTests:TestBase
    {
        [TestCaseSource(typeof(EndorsementService), nameof(EndorsementService.GetWantApprovalsDetailData))]

        public async Task GetWantApprovalsDetailQueryTestAsync(string orderId)
        {
            var response = await SendAsync(new GetWantApprovalDetailsQuery { OrderId = orderId });
            Assert.IsNotNull(response);
        }
    }
}
