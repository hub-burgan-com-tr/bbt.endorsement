using System;
using System.Threading.Tasks;
using Application.Endorsements.Queries.GetWatchApprovals;
using NUnit.Framework;
using static Application.IntegrationTests.Testing;
namespace Application.IntegrationTests.Endorsements.Queries.GetWatchApprovals
{
    public class GetWatchApprovalQueryTests:TestBase
    {
        [Test]
        [TestCase("1beecf76-5529-4309-97fe-39df9e917bd3")]
        public async Task GetWatchApprovalQueryTestAsync(string InstanceId)
        {
            var command = new GetWatchApprovalQuery
            {
                InstanceId = InstanceId,
                SeekingApproval = "",
                Approver = ""

            };
            var response = await SendAsync(command);
            var instanceId = InstanceId;
            Assert.IsNotNull(instanceId);
        }
    }
}
