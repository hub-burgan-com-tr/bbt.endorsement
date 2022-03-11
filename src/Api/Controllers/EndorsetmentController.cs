using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using Application.Common.Models;
using Application.Endorsements.Commands.NewOrders;
using Application.Endorsements.Queries.GetOrderDetails;
using Application.Endorsements.Queries.GetOrders;
using Application.Endorsements.Queries.GetOrderStatuses;
using Application.Endorsements.Commands.CancelOrders;
using Application.Endorsements.Queries.GetOrderDocuments;
using Application.Endorsements.Queries.GetApprovals;
using Application.Endorsements.Queries.GetApprovalsDetails;
using Application.Endorsements.Queries.GetApprovalsDocumentList;
using Application.Endorsements.Queries.GetApprovalsFormDocumentDetail;
using Application.Endorsements.Queries.GetApprovalsPhysicallyDocumentDetails;
using Application.Endorsements.Queries.GetMyApprovals;
using Application.Endorsements.Queries.GetMyApprovalsDetails;
using Application.Endorsements.Queries.GetWantApprovals;
using Application.Endorsements.Queries.GetWantApprovalsDetails;
using Application.Endorsements.Queries.GetWatchApprovals;
using Application.Endorsements.Queries.GetWatchApprovalsDetails;
using Application.Endorsements.Commands.ApproverOrderDocuments;

namespace Api.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class EndorsementController : ApiControllerBase
    {
        [SwaggerOperation(
            Summary = "Create new endorsement order. After endorsement is created, process is started immediately.",
            Tags = new[] { "Endorsement" }
        )]
        [Route("Orders")]
        [HttpPost]
        [SwaggerResponse(201, "Success, endorsement order is created successfully", typeof(void))]
        [SwaggerResponse(460, "Approved is not found", typeof(void))]
        [SwaggerResponse(461, "Not attached any document", typeof(void))]
        [SwaggerResponse(462, "Attachemnt size too long. Allowed maximum size is 500kb", typeof(void))]

        public async Task<Response<StartResponse>> NewOrder([FromBody] StartRequest request)
        {
            return await Mediator.Send(new NewOrderCommand { StartRequest = request });
        }


        /// <param name="approver">Approver of endorsement order. Type as citizenshipnumber.</param>
        /// <param name="customer">Customer of endorsement order. Type as citizenshipnumber for retail customers and tax number for corporate customers.</param>
        [SwaggerOperation(
           Summary = "Query endorsement orders.",
           Tags = new[] { "Endorsement" }
        )]
        [Route("Orders")]
        [HttpGet]
        [SwaggerResponse(200, "Success, queried orders are returned successfully.", typeof(OrderItem[]))]
        [SwaggerResponse(204, "Success but there is no order available for the query.", typeof(void))]    
        public async Task<Response<OrderItem[]>> GetOrders(
           [FromQuery] long approver,
           [FromQuery] long customer,
           [FromQuery] long[] submitter,
           [FromQuery] long status,
           [FromQuery] string referenceProcess,
           [FromQuery] Guid referenceId,
           [FromQuery] int pageSize = 20,
           [FromQuery] int page = 0
           )
        {
            var response = await Mediator.Send(new GetOrdersQuery
            {
                Approver = approver,
                Customer = customer,
                Page = page,    
                PageSize = pageSize,
                ReferenceProcess = referenceProcess,    
                ReferenceId = referenceId,
                Status = status,
                Submitter=submitter
            });
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SwaggerOperation(
          Summary = "Query endorsement orderDetail.",
          Tags = new[] { "Endorsement" }
        )]
        [Route("Orders/{id}")]
        [HttpGet]
        [SwaggerResponse(200, "Success, order is returned successfully.", typeof(OrderDetail))]
        [SwaggerResponse(404, "Order is not found.", typeof(void))]
        public async Task<Response<OrderDetail>> GetOrderDetail(
            [FromRoute] Guid id
            )
        {
            var response = await Mediator.Send(new GetOrderDetailQuery {  Id = id  });
            return response;
        }

        [SwaggerOperation(
          Summary = "Query endorsement order status.",
          Tags = new[] { "Endorsement" }
        )]
        [Route("Orders/{id}/Status")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<Response<string>> GetOrderStatus(
            [FromRoute] Guid id
            )
        {
            var status = await Mediator.Send(new GetOrderStatusQuery { Id = id });

            return status;
        }


        [SwaggerOperation(
          Summary = "Cancel endorsement order.",
          Tags = new[] { "Endorsement" }
        )]
        [Route("Orders/{id}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<Response<bool>> CancelOrder(
            [FromRoute] Guid id
            )
        {
            return await Mediator.Send(new CancelOrderCommand { Id = id });
        }


        [SwaggerOperation(
          Summary = "Query endorsement order documents.",
          Tags = new[] { "Endorsement" }
        )]
        [Route("Orders/{orderId}/Documents")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<Response<OrderDocumentResponse>> GetOrderDocument(
            [FromRoute] Guid orderId
            )
        {
            return await Mediator.Send(new GetOrderDocumentQuery
            {
                OrderId = orderId
            });
        }


        [SwaggerOperation(
          Summary = "Query the Confirmation order documents that you want to approve",
          Tags = new[] { "Endorsement" }
        )]
        [Route("Orders/{orderId}/Documents/{documentId}/Approve")]
        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<Response<ApproverOrderDocumentResponse>> ApproveOrderDocument(
            [FromRoute] Guid orderId,
            [FromRoute] Guid documentId
            )
        {
            return await Mediator.Send(new ApproverOrderDocumentCommand { OrderId = orderId, DocumentId = documentId });
        }

        /// <summary>
        ///  Onayımdakiler Listesi
        /// </summary>
        /// <returns></returns>
        /// <response code="404">If the item is null</response> 
        /// <param name="instanceId">Approval of endorsement order. Type as instanceId.</param>
        [SwaggerOperation(
            Summary = "Query endorsement approvals.",
            Tags = new[] { "Endorsement" }
        )]
        [Route("Endorsement")]
        [HttpGet]
        [SwaggerResponse(200, "Success, queried approvals are returned successfully.", typeof(List<GetApprovalDto>))]
        [SwaggerResponse(404, "Success but there is no order available for the query.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetApprovalAsync([FromBody] string instanceId)
        {
            await Mediator.Send(new GetApprovalQuery { InstanceId = instanceId });
            return Ok();
        }
        /// <summary>
        ///  Onayımdakiler Detay sayfası
        /// </summary>
        /// <param name="approvalId"></param>
        /// <returns></returns>
        /// <response code="404">If the item is null</response>
        [SwaggerOperation(
            Summary = "Query endorsement approval detail.",
            Tags = new[] { "Endorsement" }
        )]
        [Route("approval-detail")]
        [HttpGet]
        [SwaggerResponse(200, "Success, approval detail is returned successfully.", typeof(List<GetApprovalDetailsDto>))]
        [SwaggerResponse(404, "Approval detail is not found.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetApprovalDetailAsync([FromBody] int approvalId)
        {
            await Mediator.Send(new GetApprovalDetailsQuery() { ApprovalId = approvalId });
            return Ok();
        }

        /// <summary>
        ///  Onayımdakiler Fiziksel Belge Detay sayfası
        /// </summary>
        /// <param name="approvalId"></param>
        /// <returns></returns>
        /// <response code="404">If the item is null</response>
        [SwaggerOperation(
            Summary = "Query endorsement approval physically document detail.",
            Tags = new[] { "Endorsement" }
        )]
        [Route("approval-physically-document-detail")]
        [HttpGet]
        [SwaggerResponse(200, "Success, approval physically document detail is returned successfully.", typeof(List<GetApprovalPhysicallyDocumentDetailsDto>))]
        [SwaggerResponse(404, "Approval physically document detail is not found.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetApprovalPhysicallyDocumentDetailAsync([FromBody] int approvalId)
        {
            await Mediator.Send(new GetApprovalPhysicallyDocumentDetailsQuery() { ApprovalId = approvalId });
            return Ok();
        }

        /// <summary>
        ///  Onayımdakiler form Belge Detay sayfası
        /// </summary>
        /// <param name="approvalId"></param>
        /// <returns></returns>
        /// <response code="404">If the item is null</response>
        [SwaggerOperation(
            Summary = "Query endorsement approval form document detail.",
            Tags = new[] { "Endorsement" }
        )]
        [Route("approval-form-document-detail")]
        [HttpGet]
        [SwaggerResponse(200, "Success, approval form document detail is returned successfully.", typeof(List<GetApprovalPhysicallyDocumentDetailsDto>))]
        [SwaggerResponse(404, "Approval form document detail is not found.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetApprovalFormDocumentDetailAsync([FromBody] int approvalId)
        {
          var result=  await Mediator.Send(new GetApprovalFormDocumentDetailQuery() { ApprovalId = approvalId });
            return Ok();
        }


        /// <summary>
        ///  Onayımdakiler  Belge Listesi
        /// </summary>
        /// <param name="approvalId"></param>
        /// <returns></returns>
        /// <response code="404">If the item is null</response>
        [SwaggerOperation(
            Summary = "Query endorsement approval  document list.",
            Tags = new[] { "Endorsement" }
        )]
        [Route("approval-document-list")]
        [HttpGet]
        [SwaggerResponse(200, "Success, approval  document list is returned successfully.", typeof(List<GetApprovalsDocumentListDto>))]
        [SwaggerResponse(404, "Approval  document list is not found.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetApprovalDocumentListAsync([FromBody] int approvalId)
        {
            await Mediator.Send(new GetApprovalsDocumentListQuery() { ApprovalId = approvalId });
            return Ok();
        }


        /// <summary>
        ///  Onayladıklarım Listesi
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        /// <response code="404">If the item is null</response>
        [SwaggerOperation(
            Summary = "Query endorsement my approvals.",
            Tags = new[] { "Endorsement" }
        )]
        [Route("my-approval")]
        [HttpGet]
        [SwaggerResponse(200, "Success, queried my approvals are returned successfully.", typeof(List<GetMyApprovalDto>))]
        [SwaggerResponse(404, "Success but there is no my approvals available for the query.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetMyApprovalAsync([FromBody] string instanceId)
        {
            await Mediator.Send(new GetMyApprovalQuery { InstanceId = instanceId });
            return Ok();
        }
        /// <summary>
        ///  Onayladıklarım detay sayfası
        /// </summary>
        /// <param name="approvalId"></param>
        /// <returns></returns>
        /// <response code="404">If the item is null</response>
        [SwaggerOperation(
            Summary = "Query endorsement my approval detail.",
            Tags = new[] { "Endorsement" }
        )]
        [Route("my-approval-detail")]
        [HttpGet]
        [SwaggerResponse(200, "Success, queried my approval detail are returned successfully.", typeof(List<GetMyApprovalDetailsDto>))]
        [SwaggerResponse(404, "Success but there is no my approval detail available for the query.", typeof(void))]

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetMyApprovalDetailAsync([FromBody] int approvalId)
        {
            await Mediator.Send(new GetMyApprovalDetailsQuery { ApprovalId = approvalId });
            return Ok();
        }


        /// <summary>
        ///  İstediğim Onaylar Listesi
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        /// <response code="404">If the item is null</response>
        [SwaggerOperation(
            Summary = "Query endorsement  want approvals.",
            Tags = new[] { "Endorsement" }
        )]
        [Route("want-approval")]
        [HttpGet]
        [SwaggerResponse(200, "Success, queried want approvals are returned successfully.", typeof(List<GetWantApprovalDto>))]
        [SwaggerResponse(404, "Success but there is no want approvals available for the query.", typeof(void))]

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetWantApprovalAsync([FromBody] string instanceId)
        {
            await Mediator.Send(new GetWantApprovalQuery { InstanceId = instanceId });
            return Ok();
        }

        /// <summary>
        ///  İstediğim Onaylar detay sayfası
        /// </summary>
        /// <param name="approvalId"></param>
        /// <returns></returns>
        /// <response code="404">If the item is null</response>
        [SwaggerOperation(
            Summary = "Query endorsement want approval detail.",
            Tags = new[] { "Endorsement" }
        )]
        [Route("want-approval-detail")]
        [HttpGet]
        [SwaggerResponse(200, "Success, queried want approval detail are returned successfully.", typeof(GetWantApprovalDetailsDto))]
        [SwaggerResponse(404, "Success but there is no want approval detail available for the query.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetWantApprovalDetailAsync([FromBody] int approvalId)
        {
            await Mediator.Send(new GetWantApprovalDetailsQuery() { ApprovalId = approvalId });
            return Ok();
        }

        /// <summary>
        ///  İzleme Listesi
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <response code="404">If the item is null</response>
        [SwaggerOperation(
            Summary = "Query endorsement watch approvals.",
            Tags = new[] { "Endorsement" }
        )]
        [Route("watch-approval")]
        [HttpGet]
        [SwaggerResponse(200, "Success, queried watch approvals are returned successfully.", typeof(List<GetWatchApprovalDto>))]
        [SwaggerResponse(404, "Success but there is no watch approvals available for the query.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetWatchApprovalAsync([FromBody] GetWatchApprovalQuery command)
        {
            await Mediator.Send(command);
            return Ok();
        }

        /// <summary>
        ///  İzleme Detay Sayfası
        /// </summary>
        /// <param name="approvalId"></param>
        /// <returns></returns>
        /// <response code="404">If the item is null</response>

        [SwaggerOperation(
            Summary = "Query endorsement watch approval detail.",
            Tags = new[] { "Endorsement" }
        )]
        [Route("watch-approval-detail")]
        [HttpGet]
        [SwaggerResponse(200, "Success, queried watch approval detail are returned successfully.", typeof(GetWatchApprovalDetailsDto))]
        [SwaggerResponse(404, "Success but there is no watch approval detail  available for the query.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetWatchApprovalDetailAsync([FromBody] int approvalId)
        {
            await Mediator.Send(new GetWatchApprovalDetailsQuery() { ApprovalId = approvalId });
            return Ok();
        }

    }
}