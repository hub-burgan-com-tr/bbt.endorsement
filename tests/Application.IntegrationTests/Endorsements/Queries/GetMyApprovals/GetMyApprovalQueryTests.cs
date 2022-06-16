using System.Threading.Tasks;
using Application.Endorsements.Queries.GetMyApprovals;
using Application.IntegrationTests.Services;
using NUnit.Framework;
using static Application.IntegrationTests.Testing;

namespace Application.IntegrationTests.Endorsements.Queries.GetMyApprovals
{
    public class GetMyApprovalQueryTests:TestBase
    {
        [TestCaseSource(typeof(EndorsementService), nameof(EndorsementService.GetMyApprovalQueryData))]

        public async Task GetMyApprovalQueryTestAsync(string orderId)
        {
            var response = await SendAsync(new GetMyApprovalQuery { PageNumber=1,PageSize=10 });
            Assert.IsNotNull(response);
        }
    }
}
