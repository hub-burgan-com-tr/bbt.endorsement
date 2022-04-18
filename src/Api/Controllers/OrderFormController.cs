using Application.Common.Models;
using Application.Endorsements.Commands.NewOrderForms;
using Application.OrderForms.Queries.GetFormContents;
using Application.OrderForms.Queries.GetForms;
using Application.OrderForms.Queries.GetProcess;
using Application.OrderForms.Queries.GetProcessAndState;
using Application.OrderForms.Queries.GetStates;
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
          Summary = "Get state by name",
          Description = "Returns state by name"
      )]
        [Route("GetState")]
        [HttpGet]
        [SwaggerResponse(200, "Success, state is returned successfully.", typeof(List<GetStateDto>))]
        [SwaggerResponse(404, "State not found.", typeof(void))]
        public async Task<IActionResult> GetStateAsync()
        {
            var list = await Mediator.Send(new GetStateQuery { ParameterTypeId=(int)ParameterType.State});
            return Ok(list);
        }

        [SwaggerOperation(
          Summary = "Get process by name",
          Description = "Returns process by name"
      )]
        [Route("GetProcess")]
        [HttpGet]
        [SwaggerResponse(200, "Success, Process is returned successfully.", typeof(List<GetProcessDto>))]
        [SwaggerResponse(404, "Process not found.", typeof(void))]
        public async Task<IActionResult> GetProcessAsync()
        {
            var list = await Mediator.Send(new GetProcessQuery {ParameterTypeId=(int)ParameterType.Process});
            return Ok(list);
        }

        [SwaggerOperation(
       Summary = "Get process and state by name",
       Description = "Returns process and state by name")]
        [Route("GetProcessAndState")]
        [HttpGet]
        [SwaggerResponse(200, "Success, Process And State is returned successfully.", typeof(GetProcessAndStateDto))]
        [SwaggerResponse(404, "Process And State not found.", typeof(void))]
        public async Task<IActionResult> GetProcessAndStateAsync()
        {
            var list = await Mediator.Send(new GetProcessAndStateQuery { ProcessParameterTypeId = (int)ParameterType.Process,StateParameterTypeId=(int)ParameterType.State });
            return Ok(list);
        }

    }
}
