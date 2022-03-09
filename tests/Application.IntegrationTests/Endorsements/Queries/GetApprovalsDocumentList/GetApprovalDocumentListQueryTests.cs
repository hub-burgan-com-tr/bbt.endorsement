using System.Threading.Tasks;
using Application.Endorsements.Queries.GetApprovalsDocumentList;
using NUnit.Framework;
using static Application.IntegrationTests.Testing;

namespace Application.IntegrationTests.Endorsements.Queries.GetApprovalsDocumentList
{
    public class GetApprovalDocumentListQueryTests:TestBase
    {
        [Test]
        [TestCase("1")]
        public async Task GetApprovalDocumentListQueryTestAsync(int approvalId)
        {
            var response = await SendAsync(new GetApprovalsDocumentListQuery { ApprovalId = approvalId });
            Assert.IsNotNull(approvalId);
        }
    }
}
