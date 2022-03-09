using System.Threading.Tasks;
using Application.Endorsements.Queries.GetMyApprovals;
using NUnit.Framework;
using static Application.IntegrationTests.Testing;

namespace Application.IntegrationTests.Endorsements.Queries.GetMyApprovals
{
    public class GetMyApprovalQueryTests:TestBase
    {
        [Test]
        [TestCase("1beecf76-5529-4309-97fe-39df9e917bd3")]
        public async Task GetMyApprovalQueryTestAsync(string instanceId)
        {
            var response = await SendAsync(new GetMyApprovalQuery { InstanceId = instanceId });
            Assert.IsNotNull(instanceId);
        }
    }
}
