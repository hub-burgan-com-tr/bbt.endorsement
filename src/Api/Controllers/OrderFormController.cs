using System.Net;
using Application.OrderForms.Commands.CreateOrderFormCommands;
using Application.OrderForms.Queries.GetForms;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(
            Summary = "Query endorsement form commands.",
            Tags = new[] { "Endorsement Form Command" }
        )]
        [Route("form")]
        [HttpGet]
        [SwaggerResponse(200, "Success, queried form commands are returned successfully.", typeof(List<GetFormDto>))]
        [SwaggerResponse(404, "Success but there is no form commands available for the query.", typeof(void))]
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
        [SwaggerOperation(
            Summary = "Create new endorsement form commands. After endorsement is created, process is started immediately.",
            Tags = new[] { "Endorsement Form Command" }
        )]
        [Route("create")]
        [HttpPost]
        [SwaggerResponse(201, "Success, endorsement form command is created successfully", typeof(bool))]
        [SwaggerResponse(400, "Form command is not found", typeof(void))]
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
