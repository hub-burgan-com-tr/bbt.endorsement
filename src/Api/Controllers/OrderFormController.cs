using Application.Common.Models;
using Application.Endorsements.Commands.NewOrders;
using Application.OrderForms.Commands.CreateOrUpdateForms;
using Application.OrderForms.Queries.GetApproverInformation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    /// <summary>
    /// Form İşlemleri 
    /// </summary>
    [Route("Forms")]
    [ApiController]
    public class FormController : ApiControllerBase
    {
        [SwaggerOperation(
            Summary = "Creates or updates form definition",
            Description = "Form definitons are stored as a form.io schema. All forms stored with tag data. Tags are primary query elements of form"
        )]
        [Route("")]
        [HttpPost]
        [SwaggerResponse(200, "Success, form is updated successfully.", typeof(void))]
        [SwaggerResponse(201, "Success, form is created successfully.", typeof(void))]
        public async Task<Response<StartResponse>> CreateOrUpdateFormAsync([FromBody] StartFormRequest request)
        {
            request.Id = Guid.NewGuid();

            var documents = new List<DocumentClass>();
            documents.Add(new DocumentClass
            {
                Content = request.Content,
                Title = request.Title,
            });
            var startRequest = new StartRequest
            {
                Approver = request.Approver,
                Config = null, // Sabit değer
                Reference = request.Reference,
                Title = request.Title,
                Id = request.Id,
                Documents = documents
            };

            return await Mediator.Send(new NewOrderCommand { StartRequest = startRequest });
        }

        [SwaggerOperation(
           Summary = "Get forms by tag",
           Description = "Get forms by tag"
       )]
        [Route("")]
        [HttpGet]
        [SwaggerResponse(200, "Success, forms are returned successfully.", typeof(FormDefinition[]))]
        [SwaggerResponse(204, "There is not available any form.", typeof(void))]
        public async Task<IActionResult> GetByTagFormAsync([FromQuery] string[] tags)
        {
            return Ok();
        }
       
        [SwaggerOperation(
           Summary = "Get form by name",
           Description = "Returns form by name"
       )]
        [Route("{name}")]
        [HttpGet]
        [SwaggerResponse(200, "Success, form is returned successfully.", typeof(FormDefinition))]
        [SwaggerResponse(404, "Form not found.", typeof(void))]
        public async Task<IActionResult> GetFormAsync([FromRoute] string  name)
        {
            return Ok();
        }

        /// <summary>
        /// Onaycı Bilgileri Getir
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [SwaggerOperation(
            Summary = "Get approver by name and surname",
            Description = "Returns form by name"
        )]
        [Route("approval-information")]
        [HttpGet]
        [SwaggerResponse(200, "Success, form is returned successfully.", typeof(string))]
        [SwaggerResponse(404, "Approver not found.", typeof(void))]
        public async Task<IActionResult> GetApproverInformationAsync([FromQuery] int type,[FromQuery]string value)
        {
            await Mediator.Send(new GetApproverInformationQuery { Type = type,Value = value});
            return Ok();
        }
        /// <summary>
        /// Form ile Emir Ekleme
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [SwaggerOperation(
            Summary = "Creates order form definition",
            Description = "Create new  order form. After endorsement is created, process is started immediately."
        )]
        [Route("order-form")]
        [HttpPost]
        [SwaggerResponse(200, "Success, form is updated successfully.", typeof(void))]
        [SwaggerResponse(201, "Success, form is created successfully.", typeof(void))]
        public async Task<IActionResult> CreateNewOrderFormAsync([FromBody] CreateOrUpdateFormCommand data)
        {
            await Mediator.Send(data);
            return Ok();
        }
    }
}
