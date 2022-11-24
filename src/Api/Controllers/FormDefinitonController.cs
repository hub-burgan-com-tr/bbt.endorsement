﻿using Api.Extensions;
using Application.Common.Models;
using Application.OrderForms.Commands.CreateDependencyFormInformations;
using Application.OrderForms.Commands.CreateFormInformations;
using Application.OrderForms.Commands.CreateFormInformationsText;
using Application.OrderForms.Commands.UpdateFormDependencyReuse;
using Application.OrderForms.Commands.UpdateFormInformations;
using Application.OrderForms.Commands.UpdateFormioFormInformations;
using Application.OrderForms.Queries.GetFormInformations;
using Application.Orders.Queries.GetOrderByFormIds;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    //[Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("FormDefinition")]
    [ApiController]
    public class FormDefinitionController : ApiControllerBase
    {
        [SwaggerOperation(
          Summary = "create form definition",
          Description = "Form definitons create form information"
        )]
        [Route("CreateFormInformation")]
        [HttpPost]
        [SwaggerResponse(200, "Success, form is updated successfully.", typeof(bool))]
        [SwaggerResponse(201, "Success, form is created successfully.", typeof(bool))]
        public async Task<Response<bool>> CreateFormInformation([FromForm] CreateFormInformationCommand request)
        {
            return await Mediator.Send(request);
        }



        [SwaggerOperation(
          Summary = "create form definition",
          Description = "Form definitons create form information text"
        )]
        [Route("CreateFormInformationText")]
        [HttpPost]
        [SwaggerResponse(200, "Success, form is updated successfully.", typeof(bool))]
        [SwaggerResponse(201, "Success, form is created successfully.", typeof(bool))]
        public async Task<Response<bool>> CreateFormInformationText([FromForm] CreateFormInformationTextCommand request)
        {
            return await Mediator.Send(request);
        }






        [SwaggerOperation(
          Summary = "create form definition",
          Description = "Form definitons create form information"
        )]
        [Route("CreateDependencyFormInformation")]
        [HttpPost]
        [SwaggerResponse(200, "Success, form is updated successfully.", typeof(bool))]
        [SwaggerResponse(201, "Success, form is created successfully.", typeof(bool))]
        public async Task<Response<bool>> CreateDependencyFormInformation([FromForm] CreateDependencyFormInformationCommand request)
        {
            return await Mediator.Send(request);
        }

        [SwaggerOperation(
            Summary = "updates form definition",
            Description = "Form definitons update config information"
        )]
        [Route("UpdateFormInformation")]
        [HttpPost]
        [SwaggerResponse(200, "Success, form is updated successfully.", typeof(bool))]
        [SwaggerResponse(201, "Success, form is created successfully.", typeof(bool))]
        public async Task<Response<bool>> UpdateFormInformation([FromForm] UpdateFormInformationCommand request)
        {
            return await Mediator.Send(request);
        }

        [Route("UpdateFormioFormInformation")]
        [HttpPost]
        [SwaggerResponse(200, "Success, form is updated successfully.", typeof(bool))]
        [SwaggerResponse(201, "Success, form is created successfully.", typeof(bool))]
        public async Task<Response<bool>> UpdateFormioFormInformation([FromForm] UpdateFormioFormInformationCommand request)
        {
            return await Mediator.Send(request);
        }




        [SwaggerOperation(
         Summary = "updates form definition dependency reuse",
         Description = "Form definitons update dependency reuse information"
        )]
        [Route("UpdateFormDependencyReuse")]
        [HttpPost]
        [SwaggerResponse(200, "Success, form is updated successfully.", typeof(bool))]
        [SwaggerResponse(201, "Success, form is created successfully.", typeof(bool))]
        public async Task<Response<bool>> UpdateFormDependencyReuse([FromBody] UpdateFormDependencyReuseCommand request)
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
              Summary = "Get orders",
              Description = "Returns orders")]
        [Route("GetOrderByFormId")]
        [HttpPost]
        [SwaggerResponse(200, "Orders is returned successfully.", typeof(List<GetOrderByFormIdResponse>))]
        [SwaggerResponse(404, "Orders not found.", typeof(void))]
        public async Task<IActionResult> GetOrderByFormId([FromBody] GetOrderByFormIdQuery request)
        {
            var list = await Mediator.Send(request);
            return Ok(list);
        }
    }
}
