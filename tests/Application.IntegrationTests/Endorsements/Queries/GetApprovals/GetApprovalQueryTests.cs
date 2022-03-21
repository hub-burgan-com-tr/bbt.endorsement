using System.Threading.Tasks;
using Application.Endorsements.Queries.GetApprovals;
using NUnit.Framework;
using static Application.IntegrationTests.Testing;

namespace Application.IntegrationTests.Endorsements.Queries.GetApprovals
{
    public class GetApprovalQueryTests:TestBase
    {

        [Test]
        [TestCase("fa5bac5d-4f61-4637-a8cf-40e51d5de75c")]
        public async Task GetApprovalQueryTestAsync(string orderId)
        {
            var response = await SendAsync(new GetApprovalQuery { PageNumber=1,PageSize=10 });
            Assert.IsNotNull(response);
        }
       
    }
}
