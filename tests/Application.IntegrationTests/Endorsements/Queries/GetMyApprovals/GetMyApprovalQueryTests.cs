using System.Threading.Tasks;
using Application.Endorsements.Queries.GetMyApprovals;
using NUnit.Framework;
using static Application.IntegrationTests.Testing;

namespace Application.IntegrationTests.Endorsements.Queries.GetMyApprovals
{
    public class GetMyApprovalQueryTests:TestBase
    {
        [Test]
        [TestCase("fa5bac5d-4f61-4637-a8cf-40e51d5de75c")]
        public async Task GetMyApprovalQueryTestAsync(string orderId)
        {
            var response = await SendAsync(new GetMyApprovalQuery { OrderId = orderId });
            Assert.IsNotNull(response);
        }
    }
}
