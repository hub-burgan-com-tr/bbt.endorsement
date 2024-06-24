using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Endorsements.Queries.GetWantApprovals;
using Application.IntegrationTests.Services;
using NUnit.Framework;
using static Application.IntegrationTests.Testing;

namespace Application.IntegrationTests.Endorsements.Queries.GetWantApprovals
{
    public class GetWantApprovalQueryTests:TestBase
    {
        [TestCaseSource(typeof(EndorsementService), nameof(EndorsementService.GetWantApprovalQueryData))]

        public async Task GetWantApprovalQueryTestAsync(string orderId)
        {
            var response = await SendAsync(new GetWantApprovalQuery { PageNumber=1,PageSize=10 });
            //Assert.IsNotNull(response);
        }
    }
}
