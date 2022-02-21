using System.Net;
using Application.Documents.Commands.CreateDocumentCommands;
using Application.Documents.Commands.DeleteDocumentCommands;
using Application.Documents.Commands.UpdateDocumentCommands;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

    /// <summary>
    /// Belge İşlemleri 
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DocumentController : ApiControllerBase
    {
        #region Belge Ekleme Güncelleme Ve Silme
        #region Belge Ekleme
        /// <summary>
        ///  Belge Ekleme
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Response</returns>
        /// <response code="400">If the item is null</response>
        [Route("create")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateDocumentAsync([FromBody] CreateDocumentCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }
        #endregion
        /// <summary>
        /// Belge Güncelleme
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Response</returns>
        /// <response code="400">If the item is null</response>
        #region Belge Guncelleme
        [Route("update")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateDocumentAsync([FromBody] UpdateDocumentCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }
        #endregion
        #region Belge Silme 
        /// <summary>
        /// Belge Silme
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Response</returns>
        /// <response code="400">If the item is null</response>

        [Route("delete")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteDocumentAsync([FromBody] DeleteDocumentCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }  
        #endregion
        #endregion
    }






    
}
