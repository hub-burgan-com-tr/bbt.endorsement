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
using Application.Endorsements.Queries.GetMyApprovals;
using Application.Endorsements.Queries.GetMyApprovalsDetails;
using Application.Endorsements.Queries.GetWantApprovals;
using Application.Endorsements.Queries.GetWantApprovalsDetails;
using Application.Endorsements.Queries.GetWatchApprovals;
using Application.Endorsements.Queries.GetWatchApprovalsDetails;
using Application.Endorsements.Commands.ApproveOrderDocuments;
using Domain.Models;
using Domain.Enums;
using Api.Extensions;
using Application.Endorsements.Queries.GetDocumentPdfs;
using System.Net.Http.Headers;

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
            if (!User.IsCredentials(Request.Headers["R-User-Name"]))
                return Response<StartResponse>.Fail("Yetkiniz bulunmuyor.", 200);

            request.Id = Guid.NewGuid().ToString();
            var person = UserExtensions.GetOrderPerson(User.Claims);
            return await Mediator.Send(new NewOrderCommand { StartRequest = request, Person= person, FormType = Form.Order });
        }

        [SwaggerOperation(
            Summary = "approve order document",
            Description = "order documents that you want to approve"
        )]
        [Route("approve-order-document")]
        [HttpPost]
        [SwaggerResponse(200, "Success, form is updated successfully.", typeof(bool))]
        [SwaggerResponse(201, "Success, form is created successfully.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<Response<bool>> ApproveOrderDocument(ApproveOrderDocumentCommand command)
        {
            return await Mediator.Send(command);
        }

        [SwaggerOperation(
          Summary = "cancel order",
          Description = "canceled order")]
        [Route("cancel-order")]
        [HttpPost]
        [SwaggerResponse(200, "Success, cancel order successfully.", typeof(bool))]
        [SwaggerResponse(201, "Success, cancel order successfully.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<Response<bool>> CancelOrder([FromBody]
            CancelOrderCommand data)
        {
            return await Mediator.Send(new CancelOrderCommand { orderId = data.orderId });
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
                Submitter = submitter
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
            var response = await Mediator.Send(new GetOrderDetailQuery { Id = id });
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
        /// <summary>
        ///  Onayımdakiler Listesi
        /// </summary>
        /// <returns></returns>
        /// <response code="404">If the item is null</response> 
        /// <param name="orderId">Approval of endorsement order. Type as orderId.</param>
        /// <param name="pageNumber">1</param>
        /// <param name="pageSize">10</param>
        [SwaggerOperation(
            Summary = "Query endorsement approvals.",
            Tags = new[] { "Endorsement" }
        )]
        [Route("Endorsement")]
        [HttpGet]
        [SwaggerResponse(200, "Success, queried approvals are returned successfully.", typeof(PaginatedList<GetApprovalDto>))]
        [SwaggerResponse(404, "Success but there is no order available for the query.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetApprovalAsync(int pageNumber = 1, int pageSize = 10)
        {
            var citizenshipNumber = User.GetCitizenshipNumber();
            var list = await Mediator.Send(new GetApprovalQuery { CitizenshipNumber = citizenshipNumber, PageNumber = pageNumber, PageSize = pageSize });
            return Ok(list);
        }
        /// <summary>
        ///  Onayımdakiler Detay sayfası
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <response code="404">If the item is null</response>
        [SwaggerOperation(
            Summary = "Query endorsement approval detail.",
            Tags = new[] { "Endorsement" }
        )]
        [Route("approval-detail")]
        [HttpGet]
        [SwaggerResponse(200, "Success, approval detail is returned successfully.", typeof(GetApprovalDetailsDto))]
        [SwaggerResponse(404, "Approval detail is not found.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetApprovalDetailAsync([FromQuery] string orderId)
        {
            var citizenshipNumber = User.GetCitizenshipNumber();

            var result = await Mediator.Send(new GetApprovalDetailsQuery() { CitizenshipNumber = citizenshipNumber, OrderId = orderId });
            return Ok(result);
        }
       

        /// <summary>
        ///  Onayladıklarım Listesi
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <response code="404">If the item is null</response>
        [SwaggerOperation(
            Summary = "Query endorsement my approvals.",
            Tags = new[] { "Endorsement" }
        )]
        [Route("my-approval")]
        [HttpGet]
        [SwaggerResponse(200, "Success, queried my approvals are returned successfully.", typeof(PaginatedList<GetMyApprovalDto>))]
        [SwaggerResponse(404, "Success but there is no my approvals available for the query.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetMyApprovalAsync(int pageNumber = 1, int pageSize = 10)
        {
            var citizenshipNumber = User.GetCitizenshipNumber();
            var data = await Mediator.Send(new GetMyApprovalQuery { CitizenshipNumber = citizenshipNumber, PageNumber = pageNumber, PageSize = pageSize });
            return Ok(data);
        }
        /// <summary>
        ///  Onayladıklarım detay sayfası
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <response code="404">If the item is null</response>
        [SwaggerOperation(
            Summary = "Query endorsement my approval detail.",
            Tags = new[] { "Endorsement" }
        )]
        [Route("my-approval-detail")]
        [HttpGet]
        [SwaggerResponse(200, "Success, queried my approval detail are returned successfully.", typeof(GetMyApprovalDetailsDto))]
        [SwaggerResponse(404, "Success but there is no my approval detail available for the query.", typeof(void))]

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetMyApprovalDetailAsync([FromQuery] string orderId)
        {
           // var citizenshipNumber = long.Parse(User.Claims.FirstOrDefault(c => c.Type == "CitizenshipNumber").Value);
            var citizenshipNumber = User.GetCitizenshipNumber();

            var result = await Mediator.Send(new GetMyApprovalDetailsQuery { CitizenshipNumber = citizenshipNumber, OrderId = orderId });
            return Ok(result);
        }


        /// <summary>
        ///  İstediğim Onaylar Listesi
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        /// <response code="404">If the item is null</response>
        [SwaggerOperation(
            Summary = "Query endorsement  want approvals.",
            Tags = new[] { "Endorsement" }
        )]
        [Route("want-approval")]
        [HttpGet]
        [SwaggerResponse(200, "Success, queried want approvals are returned successfully.", typeof(PaginatedList<GetWantApprovalDto>))]
        [SwaggerResponse(404, "Success but there is no want approvals available for the query.", typeof(void))]

        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetWantApprovalAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (!User.IsCredentials(Request.Headers["R-User-Name"]))
                return Ok(Response<PaginatedList<GetWantApprovalDto>>.Fail("Yetkiniz bulunmuyor.", 200));
            var orderPerson = UserExtensions.GetOrderPerson(User.Claims);

            var result = await Mediator.Send(new GetWantApprovalQuery { Person = orderPerson, PageNumber = pageNumber, PageSize = pageSize });
            return Ok(result);
        }

        /// <summary>
        ///  İstediğim Onaylar detay sayfası
        /// </summary>
        /// <param name="orderId"></param>
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
        public async Task<IActionResult> GetWantApprovalDetailAsync([FromQuery] string orderId)
        {
            if (!User.IsCredentials(Request.Headers["R-User-Name"]))
                return Ok(Response<GetWantApprovalDetailsDto>.Fail("Yetkiniz bulunmuyor.", 200));
            var citizenshipNumber = User.GetCitizenshipNumber();
            var result = await Mediator.Send(new GetWantApprovalDetailsQuery() {CitizenshipNumber=citizenshipNumber, OrderId = orderId });
            return Ok(result);
        }

        /// <summary>
        ///  İzleme Listesi
        /// </summary>
        /// <param name="approver"></param>
        /// <param name="approval"></param>
        /// <param name="process"></param>
        /// <param name="SeekingApproval"></param>
        /// <param name="Process"></param>
        /// <param name="state"></param>
        /// <param name="processNo"></param>
        /// <param name="ProcessNo"></param>
        /// <param name="command"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <response code="404">If the item is null</response>
        [SwaggerOperation(
            Summary = "Query endorsement watch approvals.",
            Tags = new[] { "Endorsement" }
        )]
        [Route("watch-approval")]
        [HttpGet]
        [SwaggerResponse(200, "Success, queried watch approvals are returned successfully.", typeof(PaginatedList<GetWatchApprovalDto>))]
        [SwaggerResponse(404, "Success but there is no watch approvals available for the query.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetWatchApprovalAsync(string customer, string approver, string process, string state,
         string processNo, int pageNumber = 1, int pageSize = 10)
        {
            if (!User.IsCredentials(Request.Headers["R-User-Name"]))
                return Ok(Response<PaginatedList<GetWatchApprovalDto>>.Fail("Yetkiniz bulunmuyor.", 200));
            var orderPerson = UserExtensions.GetOrderPerson(User.Claims);

            var result = await Mediator.Send(new GetWatchApprovalQuery { Approver = approver, Customer = customer, Process = process, State = state, ProcessNo = processNo, PageNumber = pageNumber, PageSize = pageSize,Person=orderPerson });
            return Ok(result);
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
        public async Task<IActionResult> GetWatchApprovalDetailAsync([FromQuery] string orderId)
        {
            if (!User.IsCredentials(Request.Headers["R-User-Name"]))
                return Ok(Response<GetWatchApprovalDetailsDto>.Fail("Yetkiniz bulunmuyor.", 200));
            var response = await Mediator.Send(new GetWatchApprovalDetailsQuery() { OrderId = orderId });
            return Ok(response);
        }


        [SwaggerOperation(
             Summary = "Get document pdf",
             Tags = new[] { "Endorsement" }
         )]
        [Route("get-document-pdf")]
        [HttpGet]
        [SwaggerResponse(200, "", typeof(GetWatchApprovalDetailsDto))]
        [SwaggerResponse(404, "", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetDocumentPdfAsync([FromQuery] string orderId, string documentId)
        {
            var response = await Mediator.Send(new GetDocumentPdfQuery { OrderId = orderId, DocumentId = documentId });
            return File(response.Data.Bytes, System.Net.Mime.MediaTypeNames.Application.Octet, response.Data.FileName);

            //HttpResponseMessage result = new HttpResponseMessage();
            //try
            //{
            //    result.StatusCode = HttpStatusCode.OK;
            //    result.Content = new ByteArrayContent(response.Data.Bytes);
            //    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            //    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = response.Data.FileName };
            //}
            //catch (Exception e)
            //{
            //    result.StatusCode = HttpStatusCode.InternalServerError;
            //    result.ReasonPhrase = e.Message;
            //}

            //return Ok(result);
        }

    }
}