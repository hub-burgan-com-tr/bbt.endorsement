using System.Net;
using Application.OrderForms.Commands.CreateOrderFormCommands;
using Application.OrderForms.Queries.GetForms;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// Form İşlemleri 
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderFormController : ApiControllerBase
    {
        #region Form Listese Ve Ekleme 
        #region Form Listesi
        /// <summary>
        ///  Form Listesi
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns>Response</returns>
        /// <response code="404">If the item is null</response>
        [Route("form")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetFormCommandAsync([FromBody] string instanceId)
        {
            await Mediator.Send(new GetFormQuery { InstanceId = instanceId });
            return Ok();
        }
        #endregion
        #region Form ile Emir Ekle
        /// <summary>
        ///  Form ile Emir Ekle
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns>Response</returns>
        /// <response code="404">If the item is null</response>
        [Route("create")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateFormCommandAsync([FromBody] CreateOrderFormCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }  
        #endregion
        #endregion

    }
}
