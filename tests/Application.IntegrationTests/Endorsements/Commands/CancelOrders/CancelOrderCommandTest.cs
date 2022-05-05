using Application.Endorsements.Commands.CancelOrders;
using Application.IntegrationTests.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IntegrationTests.Endorsements.Commands.CancelOrders
{
    using static Testing;

    public class CancelOrderCommandTest:TestBase
    {
        [TestCaseSource(typeof(EndorsementService), nameof(EndorsementService.CancelOrderTestData))]

        public async Task CancelOrderTest(CancelOrderCommand request)
        {
            var response = await SendAsync(request);
            Assert.IsNotNull(response.Data);
        }
    }
}
