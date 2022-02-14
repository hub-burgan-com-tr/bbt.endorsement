using System.Net;
using Application.ApproverForms.Commands.CreateApproverFormCommands;
using Application.OrderForms.Commands.CreateOrderFormCommands;
using Application.OrderForms.Queries.GetForms;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ApproverFormController :  ApiControllerBase
    {
        [Route("formcommands")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetFormCommandAsync([FromBody] string instanceId)
        {
            await Mediator.Send(new GetFormQuery { InstanceId = instanceId });
            return Ok();
        }
        [Route("createformcommand")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateFormCommandAsync([FromBody] CreateOrderFormCommand input)
        {
            await Mediator.Send(input);
            return Ok();
        }


        [Route("createapproverformcommand")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateApproverFormCommandAsync([FromBody] CreateApproverFormCommand input)
        {
            await Mediator.Send(input);
            return Ok();
        }
    }
}
