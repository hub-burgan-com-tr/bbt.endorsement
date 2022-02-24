using Application.Endorsements.Commands.NewOrders;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Application.IntegrationTests.Endorsements.Commands.NewOrders;
using static Testing;

public class NewOrderCommandTests : TestBase
{
    [Test]
    public async Task ExecuteAsync()
    {
        var command = new NewOrderCommand();
        var response = await SendAsync(command);
        response.Data.InstanceId.Should().BeEmpty();
    }
}

