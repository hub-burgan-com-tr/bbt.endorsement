using System.Threading.Tasks;
using Application.Endorsements.Queries.GetWantApprovalsDetails;
using NUnit.Framework;
using static Application.IntegrationTests.Testing;

namespace Application.IntegrationTests.Endorsements.Queries.GetWantApprovalsDetails
{
    public class GetWantApprovalsDetailQueryTests:TestBase
    {
        [Test]
        [TestCase("fa5bac5d-4f61-4637-a8cf-40e51d5de75c")]
        public async Task GetWantApprovalsDetailQueryTestAsync(int approvalId)
        {
            var response = await SendAsync(new GetWantApprovalDetailsQuery { ApprovalId = approvalId });
            Assert.IsNotNull(approvalId);
        }
    }
}
