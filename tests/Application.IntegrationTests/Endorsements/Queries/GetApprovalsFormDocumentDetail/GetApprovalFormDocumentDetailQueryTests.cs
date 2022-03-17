using System.Threading.Tasks;
using Application.Endorsements.Queries.GetApprovalsFormDocumentDetail;
using NUnit.Framework;
using static Application.IntegrationTests.Testing;

namespace Application.IntegrationTests.Endorsements.Queries.GetApprovalsFormDocumentDetail
{
    public class GetApprovalFormDocumentDetailQueryTests:TestBase
    {
        [Test]
        [TestCase("1")]
        public async Task GetApprovalFormDocumentDetailQueryTestAsync(string OrderId)
        {
            var response = await SendAsync(new GetApprovalFormDocumentDetailQuery { OrderId = OrderId });
            Assert.IsNotNull(OrderId);
        }
    }
}
