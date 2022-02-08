using Application.Approvals.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ApprovalController : ApiControllerBase
    {
        [Route("create")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateApprovalAsync([FromBody] CreateApprovalCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }
    }
}
