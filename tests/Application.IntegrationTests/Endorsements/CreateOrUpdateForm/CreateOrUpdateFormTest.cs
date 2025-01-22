using Application.Endorsements.Commands.NewOrderForms;
using Application.IntegrationTests.Services;
using Application.TemplateEngines.Commands.Renders;
using Domain.Enums;
using Domain.Models;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

using static Application.IntegrationTests.Testing;

namespace Application.IntegrationTests.Endorsements.CreateOrUpdateForm
{
    public class CreateOrUpdateFormTest : TestBase
    {
        [TestCaseSource(typeof(EndorsementService), nameof(EndorsementService.CreateOrUpdateFormTestData))]

        public async Task CreateOrUpdateFormTestAsync(StartFormRequest request)
        {

            request.Id = Guid.NewGuid().ToString();

            var form = await SendAsync(new RenderCommand { FormId = request.FormId, Content = request.Content });
            if (form.Data != null)
            {
                request.Content = form.Data.Content;
                request.RenderId = form.Data.RenderId;
            }

            var person = new OrderPerson
            {
                CitizenshipNumber = 29521547895,
                CustomerNumber = 12345678,
                First = "Mehmet",
                Last = "Güler"
            };

            var response = await SendAsync(new NewOrderFormCommand { Request = request, Person = person, FormType = Form.FormOrder });
            Assert.IsNotNull(response.Data);
        }
    }
}
