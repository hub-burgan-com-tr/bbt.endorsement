using System.Net;
using Application.ApproverForms.Commands.CreateApproverFormCommands;
using Application.OrderForms.Commands.CreateOrderFormCommands;
using Application.OrderForms.Queries.GetForms;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// Onaycı İşlemleri
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ApproverFormController :  ApiControllerBase
    {
        /// <summary>
        ///  Onaycı Ekleme
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Response</returns>
        /// <response code="400">If the item is null</response>
        [Route("create")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateApproverFormCommandAsync([FromBody] CreateApproverFormCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }


    }
}
