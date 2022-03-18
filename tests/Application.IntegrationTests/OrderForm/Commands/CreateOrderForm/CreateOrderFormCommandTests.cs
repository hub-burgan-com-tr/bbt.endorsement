
namespace Application.IntegrationTests.OrderForm.Commands.CreateOrderForm;

using Application.OrderForms.Commands.CreateOrderForm;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using static Testing;
    public class CreateOrderFormCommandTests: TestBase
    {
        [Test]
        [TestCase("1beecf76-5529-4309-97fe-39df9e917bd3")]
        public async Task CreateOrderFormAsync(Guid id)
        {  
            var command = new CreateOrUpdateFormCommand()
            {  
              FormId = 1,
              Process = "1",
              ProcessNo = "2",
              State = "3",
              FormParameters=null,
              Type =1,
              Value ="29525155454",
              NameSurname="UĞUR KARATAŞ"
            };
            var response = await SendAsync(command);
            Assert.IsNotNull(id);
        }
}
