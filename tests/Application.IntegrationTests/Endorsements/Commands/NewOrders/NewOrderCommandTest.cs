using Application.Endorsements.Commands.NewOrders;
using Application.IntegrationTests;
using Application.IntegrationTests.Services;
using Domain.Enums;
using Domain.Models;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using static Application.IntegrationTests.Testing;

public class NewOrderCommandTest : TestBase
{
    [TestCaseSource(typeof(EndorsementService), nameof(EndorsementService.NewOrderTestData))]

    public async Task NewOrderTestAsync(StartRequest request)
    {

        request.Id = Guid.NewGuid().ToString();

        var person = new OrderPerson
        {
            CitizenshipNumber = 29521547895,
            CustomerNumber = 12345678,
            First = "Mehmet",
            Last = "Güler"
        };

        var response = await SendAsync(new NewOrderCommand { StartRequest = request, Person = person, FormType = Form.Order });
        Assert.IsNotNull(response.Data);
    }
}

