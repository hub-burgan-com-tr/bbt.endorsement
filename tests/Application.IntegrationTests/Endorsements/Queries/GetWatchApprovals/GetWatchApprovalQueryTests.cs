using System;
using System.Threading.Tasks;
using Application.Endorsements.Queries.GetWatchApprovals;
using Application.IntegrationTests.Services;
using NUnit.Framework;
using static Application.IntegrationTests.Testing;
namespace Application.IntegrationTests.Endorsements.Queries.GetWatchApprovals
{
    public class GetWatchApprovalQueryTests:TestBase
    {
        [TestCaseSource(typeof(EndorsementService), nameof(EndorsementService.GetWatchApprovalQueryData))]

        public async Task GetWatchApprovalQueryTestAsync(GetWatchApprovalQuery command)
        {
           
            var response = await SendAsync(command);
            //Assert.IsNotNull(response);
        }
    }
}
