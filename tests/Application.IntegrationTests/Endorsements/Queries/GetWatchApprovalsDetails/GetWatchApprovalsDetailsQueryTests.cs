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
            public async Task GetWatchApprovalsDetailsQueryTestAsync(string orderId)
            {
                var response = await SendAsync(new GetWatchApprovalDetailsQuery { OrderId = orderId });
                Assert.IsNotNull(response);
            }
    }
}
