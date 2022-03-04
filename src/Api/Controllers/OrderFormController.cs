using System.Net;
using Application.OrderForms.Commands.CreateOrderFormCommands;
using Application.OrderForms.Queries.GetForms;
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
        public async Task<IActionResult> CreateOrUpdateFormAsync([FromBody] FormDefinition data)
        {
            //await Mediator.Send(new GetFormQuery { InstanceId = instanceId });
            return Ok();
        }

        public class FormDefinition
        {
            public string Name { get; set; }
            public string Label { get; set; }
            public string[] Tags { get; set; }
            /// <summary>
            /// If form data is used for rendering a document, render data with dedicated template in template engine.
            /// </summary>
            public string TemplateName { get; set; }
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






    }
}
