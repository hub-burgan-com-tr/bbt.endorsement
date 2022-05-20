using Application.Common.Models;
using Application.Endorsements.Commands.NewOrderForms;
using Application.OrderForms.Commands.NewTagForms;
using Application.OrderForms.Commands.UpdateFormInformations;
using Application.OrderForms.Queries.GetFormContents;
using Application.OrderForms.Queries.GetFormInformations;
using Application.OrderForms.Queries.GetForms;
using Application.OrderForms.Queries.GetOrderFormParameters;
using Application.OrderForms.Queries.GetOrderFormTags;
using Application.OrderForms.Queries.GetTags;
using Application.OrderForms.Queries.GetTagsFormName;
using Application.TemplateEngines.Commands.Renders;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    /// <summary>
    /// Form İşlemleri 
    /// </summary>
    //[Authorize]
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
            request.Id = Guid.NewGuid().ToString();

            if (request.Source == "formio")
            {
                var form = await Mediator.Send(new RenderCommand { FormId = request.FormId, Content = request.Content });
                if (form.Data != null)
                    request.Content = form.Data.Content;
            }

            if(User.Claims.Count() == 0)
                return Response<NewOrderFormResponse>.Fail("Kullanıcı bulunamadu", 404);

            var person = new OrderPerson
            {
                CitizenshipNumber = long.Parse(User.Claims.FirstOrDefault(c => c.Type == "CitizenshipNumber").Value),
                ClientNumber = long.Parse(User.Claims.FirstOrDefault(c => c.Type == "ClientNumber").Value),
                First = User.Claims.FirstOrDefault(c => c.Type == "First").Value,
                Last = User.Claims.FirstOrDefault(c => c.Type == "Last").Value
            };

            return await Mediator.Send(new NewOrderFormCommand { Request = request, Person = person, FormType = Form.FormOrder });
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
        [HttpPost]
        [SwaggerResponse(200, "Success, Process And Tags form name is returned successfully.", typeof(List<GetTagsDto>))]
        [SwaggerResponse(404, "Process And Tags form name  not found.", typeof(void))]
        public async Task<IActionResult> GetTagsFormName([FromBody] GetTagsFormNameQuery request)
        {
            var list = await Mediator.Send(request);
            return Ok(list);
        }

        [SwaggerOperation(
            Summary = "updates form definition",
            Description = "Form definitons update config information"
        )]
        [Route("UpdateFormInformation")]
        [HttpPost]
        [SwaggerResponse(200, "Success, form is updated successfully.", typeof(bool))]
        [SwaggerResponse(201, "Success, form is created successfully.", typeof(bool))]
        public async Task<Response<bool>> UpdateFormInformation([FromBody] UpdateFormInformationCommand request)
        {
            return await Mediator.Send(request);
          
        }
        [SwaggerOperation(
  Summary = "Get process and form information",
  Description = "Returns process and form information")]
        [Route("GetFormInformation")]
        [HttpGet]
        [SwaggerResponse(200, "Success, Process And Form is returned successfully.", typeof(List<GetFormInformationDto>))]
        [SwaggerResponse(404, "Process And form not found.", typeof(void))]
        public async Task<IActionResult> GetFormInformation()
        {
            var list = await Mediator.Send(new GetFormInformationQuery());
            return Ok(list);
        }

        [SwaggerOperation(
        Summary = "created tags",
        Description = "created tags"
    )]
        [Route("CreatedTag")]
        [HttpPost]
        [SwaggerResponse(200, "Success, tag is created successfully.", typeof(bool))]
        [SwaggerResponse(201, "Success, tag is created successfully.", typeof(bool))]
        public async Task<Response<bool>> CreatedTag([FromBody] NewTagCommand request)
        {
            return await Mediator.Send(request);

        }

        [SwaggerOperation(
 Summary = "Get process and Order definion parameter name Process,State,Title",
 Description = "Returns process and Order definion parameter name Process,State,Title")]
        [Route("GetOrderFormParameters")]
        [HttpGet]
        [SwaggerResponse(200, "Success, Process And Form Definion Parameter Process,State,Title is returned successfully.", typeof(GetOrderFormParameterDto))]
        [SwaggerResponse(404, "Process And Tags not found.", typeof(GetOrderFormParameterDto))]
        public async Task<IActionResult> GetOrderFormParameters()
        {
            var list = await Mediator.Send(new GetOrderFormParameterQuery());
            return Ok(list);
        }


        [SwaggerOperation(
Summary = "Get process and Order form tag",
Description = "Returns process and Order form tags")]
        [Route("GetOrderFormTag")]
        [HttpGet]
        [SwaggerResponse(200, "Success, Process And Order Form Tags.", typeof(List<GetOrderFormTagDto>))]
        [SwaggerResponse(404, "Process And Tags not found.", typeof(List<GetOrderFormTagDto>))]
        public async Task<IActionResult> GetOrderFormTag()
        {
            var list = await Mediator.Send(new GetOrderFormTagQuery());
            return Ok(list);
        }






    }
}
