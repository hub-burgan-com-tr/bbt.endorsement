using Application.Common.Models;
using Application.Endorsements.Commands.NewOrderForms;
using Application.OrderForms.Queries.GetFormContents;
using Application.OrderForms.Queries.GetForms;
using Application.OrderForms.Queries.GetTags;
using Application.OrderForms.Queries.GetTagsFormName;
using Application.TemplateEngines.Commands.Renders;
using Domain.Enums;
using Domain.Models;
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
        [Route("CreateOrUpdateForm")]
        [HttpPost]
        [SwaggerResponse(200, "Success, form is updated successfully.", typeof(void))]
        [SwaggerResponse(201, "Success, form is created successfully.", typeof(void))]
        public async Task<Response<NewOrderFormResponse>> CreateOrUpdateFormAsync([FromBody] StartFormRequest request)
        {
            request.Id = Guid.NewGuid();

            var form = await Mediator.Send(new RenderCommand { FormId = request.FormId, Content = request.Content });
            if(form.Data != null)
                request.Content = form.Data.Content;

            return await Mediator.Send(new NewOrderFormCommand { Request = request, FormType = Form.FormOrder });
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
        [SwaggerResponse(200, "Success, form is returned successfully.", typeof(GetFormContentDto))]
        [SwaggerResponse(404, "Form not found.", typeof(void))]
        public async Task<IActionResult> GetFormContentAsync([FromQuery] string formDefinitionId)
        {
            var list = await Mediator.Send(new GetFormContentQuery { FormDefinitionId = formDefinitionId });
            return Ok(list);
        }
        [SwaggerOperation(
      Summary = "Get process and tags by name",
      Description = "Returns process and tags by name")]
        [Route("GetTags")]
        [HttpGet]
        [SwaggerResponse(200, "Success, Process And Tags is returned successfully.", typeof(List<GetTagsDto>))]
        [SwaggerResponse(404, "Process And Tags not found.", typeof(void))]
        public async Task<IActionResult> GetTags()
        {
            var list = await Mediator.Send(new GetTagsQuery());
            return Ok(list);
        }


        [SwaggerOperation(
Summary = "Get process and tags Form by name",
Description = "Returns process and tags form by name")]
        [Route("GetTagsFormName")]
        [HttpGet]
        [SwaggerResponse(200, "Success, Process And Tags form name is returned successfully.", typeof(List<GetTagsDto>))]
        [SwaggerResponse(404, "Process And Tags form name  not found.", typeof(void))]
        public async Task<IActionResult> GetTagsFormName([FromQuery] string formDefinitionTagId)
        {
            var list = await Mediator.Send(new GetTagsFormNameQuery { FormDefinitionTagId=formDefinitionTagId});
            return Ok(list);
        }

    }
}
