using System.Threading.Tasks;
using Application.Endorsements.Queries.GetMyApprovalsDetails;
using NUnit.Framework;
using static Application.IntegrationTests.Testing;

namespace Application.IntegrationTests.Endorsements.Queries.GetMyApprovalsDetails
{
    public class GetMyApprovalDetailsQueryTests:TestBase
    {
        [Test]
        [TestCase("1")]
        public async Task GetMyApprovalDetailsQueryTestAsync(int approvalId)
        {
            var response = await SendAsync(new GetMyApprovalDetailsQuery { ApprovalId = approvalId });
            Assert.IsNotNull(approvalId);
        }
    }
}
