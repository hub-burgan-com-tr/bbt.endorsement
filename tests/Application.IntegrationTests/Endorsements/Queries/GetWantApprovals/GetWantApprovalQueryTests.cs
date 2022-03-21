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
        [TestCase("fa5bac5d-4f61-4637-a8cf-40e51d5de75c")]
        public async Task GetWantApprovalQueryTestAsync(string orderId)
        {
            var response = await SendAsync(new GetWantApprovalQuery { OrderId = orderId });
            Assert.IsNotNull(response);
        }
    }
}
