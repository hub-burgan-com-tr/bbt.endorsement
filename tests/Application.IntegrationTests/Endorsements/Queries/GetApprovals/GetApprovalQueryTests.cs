using System.Threading.Tasks;
using Application.Endorsements.Queries.GetApprovals;
using Application.IntegrationTests.Services;
using NUnit.Framework;
using static Application.IntegrationTests.Testing;

namespace Application.IntegrationTests.Endorsements.Queries.GetApprovals;

public class GetApprovalQueryTests : TestBase
{

    [TestCaseSource(typeof(EndorsementService), nameof(EndorsementService.GetApprovalQueryData))]
    public async Task GetApprovalQueryTestAsync(string orderId)
    {
        var response = await SendAsync(new GetApprovalQuery { PageNumber = 1, PageSize = 10 });
        Assert.IsNotNull(response.Data);
    }
}

