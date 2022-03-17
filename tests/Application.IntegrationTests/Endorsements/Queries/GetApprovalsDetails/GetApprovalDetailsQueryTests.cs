using System.Threading.Tasks;
using Application.Endorsements.Queries.GetApprovalsDetails;
using NUnit.Framework;
using static Application.IntegrationTests.Testing;

namespace Application.IntegrationTests.Endorsements.Queries.GetApprovalsDetails
{
    public class GetApprovalDetailsQueryTests:TestBase
    {
        [Test]
        [TestCase("fa5bac5d-4f61-4637-a8cf-40e51d5de75c")]
        public async Task GetApprovalDetailTestAsync(string orderId)
        {
            var response = await SendAsync(new GetApprovalDetailsQuery { OrderId = orderId });
            Assert.IsNotNull(response);
        }
    }
}
