using Application.Common.Models;
using Application.Endorsements.Commands.NewOrders;
using Application.OrderForms.Queries.GetApproverInformations;
using Application.OrderForms.Queries.GetFormContents;
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
        [Route("CreateOrUpdateFormA")]
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
        [Route("GetByTagForm")]
        [HttpGet]
        [SwaggerResponse(200, "Success, forms are returned successfully.", typeof(FormDefinitionClass[]))]
        [SwaggerResponse(204, "There is not available any form.", typeof(void))]
        public async Task<IActionResult> GetByTagFormAsync([FromQuery] string[] tags)
        {
            return Ok();
        }

    //    [SwaggerOperation(
    //Summary = "Get form by name",
    //Description = "Returns form by name"
    //     )]
    //    [Route("{GetForm}")]
    //    [HttpGet]
    //    [SwaggerResponse(200, "Success, form is returned successfully.", typeof(FormDefinition))]
    //    [SwaggerResponse(404, "Form not found.", typeof(void))]
    //    public async Task<IActionResult> GetFormAsync([FromRoute] string name)
    //    {
    //        return Ok();
    //    }

        [SwaggerOperation(
           Summary = "Get form by name",
           Description = "Returns form by name"
       )]
        [Route("GetForm")]
        [HttpGet]
        [SwaggerResponse(200, "Success, form is returned successfully.", typeof(List<GetFormDto>))]
        [SwaggerResponse(404, "Form not found.", typeof(void))]
        public async Task<IActionResult> GetFormAsync()
        {
            var list = await Mediator.Send(new GetFormQuery());
            return Ok(list);
        }
        [SwaggerOperation(
          Summary = "Get form by content",
          Description = "Returns form by content"
      )]
        [Route("GetFormContent")]
        [HttpGet]
        [SwaggerResponse(200, "Success, form is returned successfully.", typeof(string))]
        [SwaggerResponse(404, "Form not found.", typeof(void))]
        public async Task<IActionResult> GetFormContentAsync([FromQuery] string formdefinitionId)
        {
            var list = await Mediator.Send(new GetFormContentQuery { FormDefinitionId = formdefinitionId });
            return Ok(list);
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
        public async Task<IActionResult> GetApproverInformationAsync([FromQuery] int type, [FromQuery] string value)
        {
            await Mediator.Send(new GetApproverInformationQuery { Type = type, Value = value });
            return Ok();
        }

    }
}
