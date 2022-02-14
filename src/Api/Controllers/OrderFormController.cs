using System.Net;
using Application.OrderForms.Commands.CreateOrderFormCommands;
using Application.OrderForms.Queries.GetForms;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderFormController : ApiControllerBase
    {
        [Route("form")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetFormCommandAsync([FromBody] string instanceId)
        {
            await Mediator.Send(new GetFormQuery { InstanceId = instanceId });
            return Ok();
        }

        [Route("create")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateFormCommandAsync([FromBody] CreateOrderFormCommand input)
        {
            await Mediator.Send(input);
            return Ok();
        }

    }
}
