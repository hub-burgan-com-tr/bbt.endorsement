using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Endorsements.Queries.GetWantApprovals;
using NUnit.Framework;
using static Application.IntegrationTests.Testing;

namespace Application.IntegrationTests.Endorsements.Queries.GetWantApprovals
{
    public class GetWantApprovalQueryTests:TestBase
    {
        [Test]
        [TestCase("1beecf76-5529-4309-97fe-39df9e917bd3")]
        public async Task GetWantApprovalQueryTestAsync(string InstanceId)
        {
            var response = await SendAsync(new GetWantApprovalQuery { InstanceId = InstanceId });
            Assert.IsNotNull(InstanceId);
        }
    }
}
