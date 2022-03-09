using Application.Endorsements.Commands.NewOrders;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Endorsements.Commands.NewApprovelOrder;

namespace Application.IntegrationTests.Endorsements.Commands.NewOrders;
using static Testing;

public class NewOrderCommandTests : TestBase
{
    [Test]
    [TestCase("1beecf76-5529-4309-97fe-39df9e917bd3")]
    public async Task NewOrderTestAsync(Guid id)
    {

        var documents = new List<Document>();
        documents.Add(new Document{ChoiceText = "1",Content = "",DocumentType = 1,Files = null,FormId = "",NameSurname = "",Options = null});
        var command = new NewApprovalOrderCommand
        {
           Approver = new Approver
               {NameSurname = "Uğur Karataş",
                   Type = 1,
                   Value = "123456789101"

               },
           Process = "1",
           ProcessNo = "2",
           ReminderCount = "3",
           ReminderFrequency = "4",
           Step = "5",
           Title = "Onay Emri",
           Validity = "6",
           Documents = documents

        };
        var response = await SendAsync(command);
        Assert.IsNotNull(id);
    }
}

