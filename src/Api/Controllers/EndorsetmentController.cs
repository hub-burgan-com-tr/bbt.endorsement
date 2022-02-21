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
        public IActionResult NewOrder([FromBody] StartRequest command)
        {
            throw new NotImplementedException();
        }

        public class StartRequest
        {
            /// <summary>
            /// Unique Id of order. Id is corrolation key of workflow also. 
            /// </summary>
            public Guid Id { get; set; }
            public ReferenceClass Reference { get; set; }
            public long Customer { get; set; }
            public long Approver { get; set; }
            public StartRequest.DocumentClass[] Documents { get; set; }
            public class DocumentClass
            {
                public string Name { get; set; }
                public string Content { get; set; }
                public ContentType Type { get; set; }
                public enum ContentType { HTML, PDF, PlainText }

                public ActionClass[] Actions { get; set; }

                public class ActionClass
                {
                    public bool IsDefault { get; set; }
                    public string Title { get; set; }
                    public ActionType Type { get; set; }
                    public enum ActionType { Approve, Reject }

                }
            }
            public class ReferenceClass
            {
                public string Process { get; set; }
                public string State { get; set; }
                public Guid Id { get; set; }
                public CallbackClass Callback { get; set; }
                public class CallbackClass
                {
                    public CalbackMode Mode { get; set; }
                    public string URL { get; set; }
                    public enum CalbackMode { Completed, Verbose }
                }
            }
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

        public IActionResult GetOrders(
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
            throw new NotImplementedException();
        }

        public class OrderItem
        {

            public Guid Id { get; set; }

            public long Customer { get; set; }
            public long Approver { get; set; }
            public ReferenceClass Reference { get; set; }

            public class ReferenceClass
            {
                public string Process { get; set; }
                public string State { get; set; }
                public Guid Id { get; set; }
            }
            public StatusType Status { get; set; }
            public enum StatusType { Completed, InProgress, Canceled, Halted }

            public DocumentClass[] Documents { get; set; }
            public class DocumentClass
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
                public StatusType Status { get; set; }
                public enum StatusType { Approved, InProgress, Rejected }
            }
        }

        [Route("Orders/{id}")]
        [HttpGet]
        [SwaggerResponse(200, "Success, order is returned successfully.", typeof(OrderDetail))]
        [SwaggerResponse(404, "Order is not found.", typeof(void))]
        public IActionResult GetOrder(
            [FromRoute] Guid id
            )
        {
            throw new NotImplementedException();
        }

        public class OrderDetail
        {
            /// <summary>
            /// Unique Id of order. Id is corrolation key of workflow also. 
            /// </summary>
            public Guid Id { get; set; }
            public ReferenceClass Reference { get; set; }
            public long Customer { get; set; }
            public long Approver { get; set; }
            public StartRequest.DocumentClass[] Documents { get; set; }
            public class DocumentClass
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
                public string Content { get; set; }
                public ContentType Type { get; set; }
                public enum ContentType { HTML, PDF, PlainText }
                public ActionClass[] Actions { get; set; }
                public class ActionClass
                {
                    public bool IsDefault { get; set; }
                    public string Title { get; set; }
                    public ActionType Type { get; set; }
                    public enum ActionType { Approve, Reject }

                }
            }
            public class ReferenceClass
            {
                public string Process { get; set; }
                public string State { get; set; }
                public Guid Id { get; set; }
                public CallbackClass Callback { get; set; }
                public class CallbackClass
                {
                    public CalbackMode Mode { get; set; }
                    public string URL { get; set; }
                    public enum CalbackMode { Completed, Verbose }
                }
            }
        }

        [Route("Orders/{id}/Status")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetOrderStatus(
            [FromRoute] Guid id
            )
        {
            throw new NotImplementedException();
        }


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