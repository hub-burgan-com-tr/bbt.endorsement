using System.Threading.Tasks;
using Application.Endorsements.Queries.GetWatchApprovalsDetails;
using NUnit.Framework;
using static Application.IntegrationTests.Testing;
namespace Application.IntegrationTests.Endorsements.Queries.GetWatchApprovalsDetails
{
    public class GetWatchApprovalsDetailsQueryTests:TestBase
    {
        
            [Test]
            [TestCase("1")]
            public async Task GetWatchApprovalsDetailsQueryTestAsync(int approvalId)
            {
                var response = await SendAsync(new GetWatchApprovalDetailsQuery { ApprovalId = approvalId });
                Assert.IsNotNull(approvalId);
            }
    }
}
