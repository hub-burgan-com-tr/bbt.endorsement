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
        
        [Route("create")]
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
