using System.Net;
using Application.NewConfirmationOrders.Commands.NewConfirmationOrderCommands;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ConfirmationOrderController : ApiControllerBase
    {
        [Route("createconfirmationcommand")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateConfirmationCommandAsync([FromBody] NewConfirmationOrderCommand input)
        {
            await Mediator.Send(input);
            return Ok();
        }
    }
}
