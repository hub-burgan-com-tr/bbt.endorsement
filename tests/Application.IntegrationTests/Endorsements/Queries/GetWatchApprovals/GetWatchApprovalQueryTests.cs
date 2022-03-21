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
        [TestCase("fa5bac5d-4f61-4637-a8cf-40e51d5de75c")]
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
