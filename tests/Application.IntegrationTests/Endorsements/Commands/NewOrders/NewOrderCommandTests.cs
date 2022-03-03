using Application.Endorsements.Commands.NewOrders;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Application.IntegrationTests.Endorsements.Commands.NewOrders;
using static Testing;

public class NewOrderCommandTests : TestBase
{
    [Test]
    [TestCase("1beecf76-5529-4309-97fe-39df9e917bd3")]
    public async Task NewOrderTestAsync(Guid id)
    {
        var command = new NewOrderCommand
        {
            StartRequest = new StartRequest
            {
                Id = id,
            }
        };
        var response = await SendAsync(command);
        var instanceId = response.Data.InstanceId.Should().BeEmpty();
        Assert.IsNotNull(instanceId);
    }
}

