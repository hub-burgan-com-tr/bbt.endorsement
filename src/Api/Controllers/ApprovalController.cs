using Application.Approvals.Commands;
using Application.Approvals.Queries.GetApprovals;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Application.Approvals.Queries.GetMyApprovals;
using Application.Approvals.Queries.GetWantApprovals;

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

        [Route("approvalsdetail")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetApprovalsDetailAsync([FromBody] int approvalId)
        {
            await Mediator.Send(new GetApprovalDetailQuery() { ApprovalId = approvalId });
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


        [Route("myapprovalsdetail")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetMyApprovalsDetailAsync([FromBody] int approvalId)
        {
            await Mediator.Send(new GetMyApprovalDetailQuery { ApprovalId = approvalId });
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

    }
}