using Application.Endorsements.Commands.NewOrders;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.IntegrationTests.Endorsements.Commands.NewOrders;

using static Application.Endorsements.Commands.NewOrders.StartRequest;
using static Testing;

public class NewOrderCommandTests : TestBase
{
    [Test]
    [TestCase("1beecf76-5529-4309-97fe-39df9e917bd3")]
    public async Task NewOrderTestAsync(Guid id)
    {
        var documents = new List<OrderDocument>();
        var command = new NewOrderCommand()
        {
          StartRequest =new StartRequest
          {
              
              Reference = new OrderReference { Callback = null, Process = "1", ProcessNo = "2", State = "3" },
              Title = "Deneme",
              Id = id,
              Config = new OrderConfig { ExpireInMinutes = 4, MaxRetryCount = 5, RetryFrequence = 6 },
              Documents = documents
          }

        };
        var response = await SendAsync(command);
        Assert.IsNotNull(id);
    }
}

