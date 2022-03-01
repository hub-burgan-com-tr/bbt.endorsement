using Application.Common.Models;
using Application.Endorsements.Commands.NewOrders;
using Application.Endorsements.Queries.GetOrderDetails;
using Application.Endorsements.Queries.GetOrders;
using Application.Endorsements.Queries.GetOrderStatuses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

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
        public IActionResult CancelOrder(
            [FromRoute] Guid id
            )
        {
            throw new NotImplementedException();
        }


        [SwaggerOperation(
          Summary = "Query endorsement order documents.",
          Tags = new[] { "Endorsement" }
        )]
        [Route("Orders/{orderId}/Documents")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetOrderDocument(
            [FromRoute] Guid orderId
            )
        {
            throw new NotImplementedException();
        }


        [SwaggerOperation(
          Summary = "Query the Confirmation order documents that you want to approve",
          Tags = new[] { "Endorsement" }
        )]
        [Route("Orders/{orderId}/Documents/{documentId}/Approve")]
        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult ApproveOrderDocument(
            [FromRoute] Guid orderId,
            [FromRoute] Guid documentId
            )
        {
            throw new NotImplementedException();
        }
    }
}