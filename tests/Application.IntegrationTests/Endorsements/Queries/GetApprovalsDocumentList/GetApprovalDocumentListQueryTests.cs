using System.Threading.Tasks;
using Application.Endorsements.Queries.GetApprovalsDocumentList;
using NUnit.Framework;
using static Application.IntegrationTests.Testing;

namespace Application.IntegrationTests.Endorsements.Queries.GetApprovalsDocumentList
{
    public class GetApprovalDocumentListQueryTests:TestBase
    {
        [Test]
        [TestCase("fa5bac5d-4f61-4637-a8cf-40e51d5de75c")]
        public async Task GetApprovalDocumentListQueryTestAsync(string orderId)
        {
            var response = await SendAsync(new GetApprovalDocumentListQuery { OrderId = orderId });
            Assert.IsNotNull(response);
        }
    }
}
