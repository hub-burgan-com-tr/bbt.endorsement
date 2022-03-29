using Application.Endorsements.Commands.CancelOrders;
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
        [Test]
        [TestCase("1beecf76-5529-4309-97fe-39df9e917bd3")]
        public async Task CancelOrderTest(string orderId)
        {
            var response = await SendAsync(new CancelOrderCommand { orderId = orderId });
            Assert.IsNotNull(orderId);
        }
    }
}
