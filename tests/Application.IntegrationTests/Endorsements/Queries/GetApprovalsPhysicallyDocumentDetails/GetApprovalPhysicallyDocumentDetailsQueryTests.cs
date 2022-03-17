using System.Threading.Tasks;
using Application.Endorsements.Queries.GetApprovalsPhysicallyDocumentDetails;
using NUnit.Framework;
using static Application.IntegrationTests.Testing;

namespace Application.IntegrationTests.Endorsements.Queries.GetApprovalsPhysicallyDocumentDetails
{
    public class GetApprovalPhysicallyDocumentDetailsQueryTests:TestBase
    {
        [Test]
        [TestCase("fa5bac5d-4f61-4637-a8cf-40e51d5de75c")]
        public async Task GetApprovalPhysicallyDocumentDetailsQueryTestAsync(string orderId)
        {
            var response = await SendAsync(new GetApprovalPhysicallyDocumentDetailsQuery { OrderId = orderId });
            Assert.IsNotNull(response);
        }
    }
}
