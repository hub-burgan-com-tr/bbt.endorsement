using Application.Approvals.Commands;
using Application.Approvals.Queries.GetApprovals;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Application.Approvals.Queries.GetApprovalsDetails;
using Application.Approvals.Queries.GetMyApprovals;
using Application.Approvals.Queries.GetMyApprovalsDetails;
using Application.Approvals.Queries.GetWantApprovals;
using Application.Approvals.Queries.GetWatchApprovals;
using Application.ApproverForms.Commands.CreateApproverFormCommands;
using Application.OrderForms.Commands.CreateOrderFormCommands;
using Application.OrderForms.Queries.GetForms;

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

        [Route("approvals")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetApprovalsAsync([FromBody] string instanceId)
        {
            await Mediator.Send(new GetApprovalQuery { InstanceId = instanceId });
            return Ok();
        }

        [Route("approvalsdetails")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetApprovalsDetailsAsync([FromBody] int approvalId)
        {
            await Mediator.Send(new GetApprovalDetailsQuery() { ApprovalId = approvalId });
            return Ok();
        }


        [Route("myapprovals")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetMyApprovalsAsync([FromBody] string instanceId)
        {
            await Mediator.Send(new GetMyApprovalQuery { InstanceId = instanceId });
            return Ok();
        }


        [Route("myapprovalsdetails")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetMyApprovalsDetailsAsync([FromBody] int approvalId)
        {
            await Mediator.Send(new GetMyApprovalDetailsQuery { ApprovalId = approvalId });
            return Ok();
        }


        [Route("wantapprovals")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetWantApprovalsAsync([FromBody] string instanceId)
        {
            await Mediator.Send(new GetWantApprovalQuery { InstanceId = instanceId });
            return Ok();
        }


        [Route("watchapprovals")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetWatchApprovalsAsync([FromBody] GetWatchApprovalQuery input)
        {
            await Mediator.Send(input);
            return Ok();
        }


        [Route("formcommands")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetFormCommandAsync([FromBody] string instanceId)
        {
            await Mediator.Send(new GetFormQuery{InstanceId = instanceId});
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