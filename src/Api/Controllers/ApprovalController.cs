using Application.Approvals.Queries.GetApprovals;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Application.Approvals.Commands.CreateApprovalCommands;
using Application.Approvals.Commands.UpdateApprovalCommands;
using Application.Approvals.Queries.GetApprovalsDetails;
using Application.Approvals.Queries.GetApprovalsDocumentDetails;
using Application.Approvals.Queries.GetMyApprovals;
using Application.Approvals.Queries.GetMyApprovalsDetails;
using Application.Approvals.Queries.GetWantApprovals;
using Application.Approvals.Queries.GetWantApprovalsDetails;
using Application.Approvals.Queries.GetWatchApprovals;
using Application.Approvals.Queries.GetWatchApprovalsDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
      
    /// <summary>
    /// Onay İşlemleri
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ApprovalController : ApiControllerBase
    {
        #region Onay Ekleme ve Güncelleme
        #region OnayEkleme
        /// <summary>
        ///  Onay Ekleme
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Response</returns>
        /// <response code="400">If the item is null</response>
        [SwaggerOperation(
            Summary = "Create new endorsement order. After endorsement is created, process is started immediately.",
            Tags = new[] { "Endorsement" }
        )]
        [Route("create")]
        [HttpPost]
        [SwaggerResponse(201, "Success, endorsement order is created successfully", typeof(CreateApprovalCommandDto))]
        [SwaggerResponse(400, "Approval is not found", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateApprovalAsync([FromBody] CreateApprovalCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }
        #endregion
        #region Onay Guncelleme
        /// <summary>
        ///  Onay Guncelle
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        [SwaggerOperation(
            Summary = "update new endorsement order. After endorsement is updated, process is started immediately.",
            Tags = new[] { "Endorsement" }
        )]
        [Route("update")]
        [HttpPut]
        [SwaggerResponse(201, "Success, endorsement order is updated successfully", typeof(bool))]
        [SwaggerResponse(400, "Approval is not found", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateApprovalAsync([FromBody] UpdateApprovalCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }
        #endregion
        #endregion
        #region Onayimdakiler Listesi ve Detay Sayfası
        #region Onayimdakiler Listesi
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
        [Route("approval")]
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
        #endregion
        #region Onayimdakiler Detay Sayfası
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
        [SwaggerResponse(200, "Success, approval detail is returned successfully.", typeof(GetApprovalDetailsDto))]
        [SwaggerResponse(404, "Approval detail is not found.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetApprovalDetailAsync([FromBody] int approvalId)
        {
            await Mediator.Send(new GetApprovalDetailsQuery() { ApprovalId = approvalId });
            return Ok();
        }

        /// <summary>
        ///  Onayımdakiler Belge Detay sayfası
        /// </summary>
        /// <param name="approvalId"></param>
        /// <returns></returns>
        /// <response code="404">If the item is null</response>
        [SwaggerOperation(
            Summary = "Query endorsement approval document detail.",
            Tags = new[] { "Endorsement" }
        )]
        [Route("approval-document-detail")]
        [HttpGet]
        [SwaggerResponse(200, "Success, approval document detail is returned successfully.", typeof(GetApprovalDocumentDetailsDto))]
        [SwaggerResponse(404, "Approval document detail is not found.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetApprovalDocumentDetailAsync([FromBody] int approvalId)
        {
            await Mediator.Send(new GetApprovalDocumentDetailsQuery() { ApprovalId = approvalId });
            return Ok();
        }

        #endregion
        #endregion
        #region Onayladıklarım Listesi ve Detay Sayfası
        #region Onayladıklarım Listesi
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
        #endregion
        #region Onayladıklarım Detay Sayfası
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
        #endregion
        #endregion
        #region İstedigim Onaylar Listesi Ve Detay Sayfası
        #region İstedigim Onaylar
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
        #endregion
        #region İstedigim Onaylar Detay Sayfası
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
        #endregion
        #endregion
        #region İzleme Listesi Ve Detay Sayfası
        #region İzleme Listesi
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
        #endregion
        #region İzleme Detay Sayfası
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
        #endregion
        #endregion
    }
}