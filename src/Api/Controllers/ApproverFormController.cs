using System.Net;
using Application.ApproverForms.Commands.CreateApproverFormCommands;
using Application.OrderForms.Commands.CreateOrderFormCommands;
using Application.OrderForms.Queries.GetForms;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    /// <summary>
    /// Onaycı İşlemleri
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ApproverFormController : ApiControllerBase
    {
        #region Onaycı Ekleme İşlemi

        /// <summary>
        ///  Onaycı Ekleme
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Response</returns>
        /// <response code="400">If the item is null</response>

        [SwaggerOperation(
            Summary = "Create new endorsement approver form. After endorsement is created, process is started immediately.",
            Tags = new[] { "Endorsement Approver Form" }
        )]
        [Route("create")]
        [HttpPost]
        [SwaggerResponse(201, "Success, endorsement approver form is created successfully", typeof(List<CreateApproverFormCommandDto>))]
        [SwaggerResponse(400, "approver form is not found", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateApproverFormCommandAsync([FromBody] CreateApproverFormCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }
        #endregion
    }
}
