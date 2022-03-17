using System.Threading.Tasks;
using Application.Endorsements.Queries.GetApprovalsFormDocumentDetail;
using NUnit.Framework;
using static Application.IntegrationTests.Testing;

namespace Application.IntegrationTests.Endorsements.Queries.GetApprovalsFormDocumentDetail
{
    public class GetApprovalFormDocumentDetailQueryTests:TestBase
    {
        [Test]
        [TestCase("fa5bac5d-4f61-4637-a8cf-40e51d5de75c")]
        public async Task GetApprovalFormDocumentDetailQueryTestAsync(string OrderId)
        {
            var response = await SendAsync(new GetApprovalFormDocumentDetailQuery { OrderId = OrderId });
            Assert.IsNotNull(response);
        }
    }
}
