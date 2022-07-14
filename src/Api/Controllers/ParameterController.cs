using Application.Common.Models;
using Application.OrderForms.Commands.CreateFormInformations;
using Application.OrderForms.Commands.UpdateFormDependencyReuse;
using Application.OrderForms.Commands.UpdateFormInformations;
using Application.OrderForms.Queries.GetFormInformations;
using Application.OrderForms.Queries.GetTags;
using Application.Orders.Queries.GetOrderByFormIds;
using Application.Parameter.Commands.CreateParameters;
using Application.Parameter.Commands.CreateTags;
using Application.Parameter.Commands.UpdateParameters;
using Application.Parameter.Commands.UpdateTags;
using Application.Parameter.Queries.GetParameters;
using Application.Parameter.Queries.GetParametersDys;
using Application.Parameter.Queries.GetParameterTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    [Authorize]
    [Route("Parameter")]
    [ApiController]
    public class ParameterController : ApiControllerBase
    {

        [SwaggerOperation(
Summary = "Get process and parameter",
Description = "Returns process and parameter")]
        [Route("GetParameter")]
        [HttpGet]
        [SwaggerResponse(200, "Success,Parameter.", typeof(List<GetParameterDto>))]
        [SwaggerResponse(404, "Parameter not found.", typeof(List<GetParameterDto>))]
        public async Task<IActionResult> GetParameter(string text)
        {
            var list = await Mediator.Send(new GetParameterQuery { Text=text});
            return Ok(list);
        }


        [SwaggerOperation(
Summary = "Get process and parameter Dys",
Description = "Returns process and parameter Dys")]
        [Route("GetParameterDys")]
        [HttpGet]
        [SwaggerResponse(200, "Success,Parameter Dys.", typeof(List<GetParameterDysDto>))]
        [SwaggerResponse(404, "Parameter Dys not found.", typeof(List<GetParameterDysDto>))]
        public async Task<IActionResult> GetParameterDys()
        {
            var list = await Mediator.Send(new GetParameterDysQuery { });
            return Ok(list);
        }


        [SwaggerOperation(
          Summary = "create parameter",
          Description = "Parameter create"
      )]
        [Route("CreateParameter")]
        [HttpPost]
        [SwaggerResponse(200, "Success, parameter is updated successfully.", typeof(bool))]
        [SwaggerResponse(201, "Success, parameter is created successfully.", typeof(bool))]
        public async Task<Response<bool>> CreateParameter([FromBody] CreateParameterCommand request)
        {
            return await Mediator.Send(request);

        }

        [SwaggerOperation(
   Summary = "update parameter",
   Description = "Parameter update"
)]
        [Route("UpdateParameter")]
        [HttpPost]
        [SwaggerResponse(200, "Success, parameter is updated successfully.", typeof(bool))]
        [SwaggerResponse(201, "Success, parameter is created successfully.", typeof(bool))]
        public async Task<Response<bool>> UpdateParameter([FromBody] UpdateParameterCommand request)
        {
            return await Mediator.Send(request);

        }
        [SwaggerOperation(
Summary = "Get process and parameter type",
Description = "Returns process and parameter type")]
        [Route("GetParameterType")]
        [HttpGet]
        [SwaggerResponse(200, "Success,Parameter type.", typeof(List<GetParameterTypeDto>))]
        [SwaggerResponse(404, "Parameter not found.", typeof(List<GetParameterTypeDto>))]
        public async Task<IActionResult> GetParameterType()
        {
            var list = await Mediator.Send(new GetParameterTypeQuery { });
            return Ok(list);
        }

        [SwaggerOperation(
Summary = "Get process and tags",
Description = "Returns process and tag type")]
        [Route("GetTag")]
        [HttpGet]
        [SwaggerResponse(200, "Success,Tags type.", typeof(List<GetTagsDto>))]
        [SwaggerResponse(404, "Tag not found.", typeof(List<GetTagsDto>))]
        public async Task<IActionResult> GetTag()
        {
            var list = await Mediator.Send(new GetTagsQuery { });
            return Ok(list);
        }

        [SwaggerOperation(
         Summary = "create tag",
         Description = "Tag create"
     )]
        [Route("CreateTag")]
        [HttpPost]
        [SwaggerResponse(200, "Success, tag is updated successfully.", typeof(bool))]
        [SwaggerResponse(201, "Success, tag is created successfully.", typeof(bool))]
        public async Task<Response<bool>> CreateTag([FromBody] CreateTagCommand request)
        {
            return await Mediator.Send(request);

        }

        [SwaggerOperation(
Summary = "update tag",
Description = "Tag update"
)]
        [Route("UpdateTag")]
        [HttpPost]
        [SwaggerResponse(200, "Success, tag is updated successfully.", typeof(bool))]
        [SwaggerResponse(201, "Success, tag is created successfully.", typeof(bool))]
        public async Task<Response<bool>> UpdateTag([FromBody] UpdateTagCommand request)
        {
            return await Mediator.Send(request);

        }

    }
}
