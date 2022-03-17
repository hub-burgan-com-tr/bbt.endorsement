using System.Threading.Tasks;
using Application.Endorsements.Queries.GetApprovalsDetails;
using NUnit.Framework;
using static Application.IntegrationTests.Testing;

namespace Application.IntegrationTests.Endorsements.Queries.GetApprovalsDetails
{
    public class GetApprovalDetailsQueryTests:TestBase
    {
        [Test]
        [TestCase("1")]
        public async Task GetApprovalDetailTestAsync(string orderId)
        {
            var response = await SendAsync(new GetApprovalDetailsQuery { OrderId = orderId });
            Assert.IsNotNull(orderId);
        }
    }
}
