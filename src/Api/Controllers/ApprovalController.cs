using Application.Approvals.Queries.GetApprovals;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Application.Approvals.Commands.CreateApprovalCommands;
using Application.Approvals.Commands.UpdateApprovalCommands;
using Application.Approvals.Queries.GetApprovalsDetails;
using Application.Approvals.Queries.GetMyApprovals;
using Application.Approvals.Queries.GetMyApprovalsDetails;
using Application.Approvals.Queries.GetWantApprovals;
using Application.Approvals.Queries.GetWantApprovalsDetails;
using Application.Approvals.Queries.GetWatchApprovals;
using Application.Approvals.Queries.GetWatchApprovalsDetails;

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
        [Route("create")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
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
        [Route("update")]
        [HttpPut]
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
        /// <param name="instanceId"></param>
        /// <returns></returns>
        /// <response code="404">If the item is null</response> 

        [Route("approval")]
        [HttpGet]
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
        [Route("approval-detail")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetApprovalDetailAsync([FromBody] int approvalId)
        {
            await Mediator.Send(new GetApprovalDetailsQuery() { ApprovalId = approvalId });
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
        [Route("my-approval")]
        [HttpGet]
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
        [Route("my-approval-detail")]
        [HttpGet]
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
        [Route("want-approval")]
        [HttpGet]
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
        [Route("want-approval-detail")]
        [HttpGet]
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
        [Route("watch-approval")]
        [HttpGet]
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
        [Route("watch-approval-detail")]
        [HttpGet]
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