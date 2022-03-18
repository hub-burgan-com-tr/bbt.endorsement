using System.Threading.Tasks;
using Application.Endorsements.Queries.GetMyApprovalsDetails;
using NUnit.Framework;
using static Application.IntegrationTests.Testing;

namespace Application.IntegrationTests.Endorsements.Queries.GetMyApprovalsDetails
{
    public class GetMyApprovalDetailsQueryTests:TestBase
    {
        [Test]
        [TestCase("fa5bac5d-4f61-4637-a8cf-40e51d5de75c")]
        public async Task GetMyApprovalDetailsQueryTestAsync(string orderId)
        {
            var response = await SendAsync(new GetMyApprovalDetailsQuery { OrderId = orderId });
            Assert.IsNotNull(response);
        }
    }
}
