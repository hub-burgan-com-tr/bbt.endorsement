using System.Net;
using Application.ApproverForms.Commands.CreateApproverFormCommands;
using Application.ApproverForms.Commands.UpdateApproverFormCommands;
using Application.ApproverForms.Queries.GetApproverForm;
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
        [SwaggerResponse(201, "Success, endorsement approver form is created successfully", typeof(int))]
        [SwaggerResponse(400, "approver form is not found", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateApproverFormCommandAsync([FromBody] CreateApproverFormCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }


        /// <summary>
        ///  Onaycı Güncelleme
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Response</returns>
        /// <response code="400">If the item is null</response>

        [SwaggerOperation(
            Summary = "Create new endorsement approver form. After endorsement is created, process is started immediately.",
            Tags = new[] { "Endorsement Approver Form" }
        )]
        [Route("update")]
        [HttpPost]
        [SwaggerResponse(201, "Success, endorsement approver form is updated successfully", typeof(int))]
        [SwaggerResponse(400, "approver form is not found", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateApproverFormCommandAsync([FromBody] UpdateApproverFormCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }

        /// <summary>
        ///  Onayci Getir
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <response code="404">If the item is null</response>
        [SwaggerOperation(
            Summary = "Query endorsement approver form detail.",
            Tags = new[] { "Endorsement" }
        )]
        [Route("detail")]
        [HttpGet]
        [SwaggerResponse(200, "Success, approver form detail is returned successfully.", typeof(GetApproverFormDto))]
        [SwaggerResponse(404, "Approver form detail is not found.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetApproverFormCommandDetailAsync([FromQuery] int AprrovalId,[FromQuery]int FormId)
        {
            await Mediator.Send(new GetApproverFormQuery() { ApprovalIdId = AprrovalId,FormId = FormId});
            return Ok();
        }
    }
}
