﻿using Application.Common.Models;
using Application.OrderForms.Commands.CreateFormInformations;
using Application.OrderForms.Commands.UpdateFormDependencyReuse;
using Application.OrderForms.Commands.UpdateFormInformations;
using Application.OrderForms.Queries.GetFormInformations;
using Application.Orders.Queries.GetOrderByFormIds;
using Application.Parameter.Commands.CreateParameters;
using Application.Parameter.Commands.UpdateParameters;
using Application.Parameter.Queries.GetParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
  
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

    }
}