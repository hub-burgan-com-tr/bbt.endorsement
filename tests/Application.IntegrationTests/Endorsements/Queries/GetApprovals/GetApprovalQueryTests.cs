using System.Threading.Tasks;
using Application.Endorsements.Queries.GetApprovals;
using NUnit.Framework;
using static Application.IntegrationTests.Testing;

namespace Application.IntegrationTests.Endorsements.Queries.GetApprovals
{
    public class GetApprovalQueryTests:TestBase
    {

        [Test]
        [TestCase("1beecf76-5529-4309-97fe-39df9e917bd3")]
        public async Task GetApprovalQueryTestAsync(string instanceId)
        {
            var response = await SendAsync(new GetApprovalQuery { InstanceId = instanceId });
            Assert.IsNotNull(instanceId);
        }
       
    }
}
