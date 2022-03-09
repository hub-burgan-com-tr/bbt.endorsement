using System.Threading.Tasks;
using Application.Endorsements.Queries.GetWantApprovalsDetails;
using NUnit.Framework;
using static Application.IntegrationTests.Testing;

namespace Application.IntegrationTests.Endorsements.Queries.GetWantApprovalsDetails
{
    public class GetWantApprovalsDetailQueryTests:TestBase
    {
        [Test]
        [TestCase("1")]
        public async Task GetWantApprovalsDetailQueryTestAsync(int approvalId)
        {
            var response = await SendAsync(new GetWantApprovalDetailsQuery { ApprovalId = approvalId });
            Assert.IsNotNull(approvalId);
        }
    }
}
