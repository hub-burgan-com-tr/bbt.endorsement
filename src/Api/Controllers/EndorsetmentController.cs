using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EndorsementController : ApiControllerBase
    {

        [Route("Orders")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult NewOrder([FromBody] StartRequest command)
        {
            throw new NotImplementedException();
        }

        public class StartRequest
        {
            public Guid Id { get; set; }
            public ReferenceClass Reference { get; set; }
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

        [Route("Orders")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetOrders(
            [FromQuery] long approver,
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

        [Route("Orders/{id}")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetOrder(
            [FromRoute] Guid id
            )
        {
            throw new NotImplementedException();
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