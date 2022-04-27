using System.Threading.Tasks;
using Application.Endorsements.Queries.GetMyApprovalsDetails;
using Application.IntegrationTests.Services;
using NUnit.Framework;
using static Application.IntegrationTests.Testing;

namespace Application.IntegrationTests.Endorsements.Queries.GetMyApprovalsDetails
{
    public class GetMyApprovalDetailsQueryTests:TestBase
    {
        [TestCaseSource(typeof(EndorsementService), nameof(EndorsementService.GetMyApprovalQueryDetailData))]

        public async Task GetMyApprovalDetailsQueryTestAsync(string orderId)
        {
            var response = await SendAsync(new GetMyApprovalDetailsQuery { OrderId = orderId });
            Assert.IsNotNull(response);
        }
    }
}
