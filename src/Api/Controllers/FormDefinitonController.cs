using Application.Common.Models;
using Application.OrderForms.Commands.CreateFormInformations;
using Application.OrderForms.Commands.UpdateFormInformations;
using Application.OrderForms.Queries.GetFormInformations;
using Application.Orders.Queries.GetOrderByFormIds;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    /// <summary>
    /// Form Definition İşlemleri 
    /// </summary>
    //[Authorize]
    [Route("FormDefiniton")]
    [ApiController]
    public class FormDefinitonController : ApiControllerBase
    {
        [SwaggerOperation(
          Summary = "cerate form definition",
          Description = "Form definitons create form information"
      )]
        [Route("CreateFormInformation")]
        [HttpPost]
        [SwaggerResponse(200, "Success, form is updated successfully.", typeof(bool))]
        [SwaggerResponse(201, "Success, form is created successfully.", typeof(bool))]
        public async Task<Response<bool>> CreateFormInformation([FromBody] CreateFormInformationCommand request)
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
