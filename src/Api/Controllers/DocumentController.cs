using System.Net;
using Application.Documents.Commands.CreateDocumentCommands;
using Application.Documents.Commands.DeleteDocumentCommands;
using Application.Documents.Commands.Queries.GetDocumentDetails;
using Application.Documents.Commands.UpdateDocumentCommands;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(
            Summary = "Create new endorsement document. After endorsement document is created, process is started immediately.",
            Tags = new[] { "Endorsement Document" }
        )]
        [Route("create")]
        [HttpPost]
        [SwaggerResponse(201, "Success, endorsement document is created successfully", typeof(List<CreateDocumentCommandDto>))]
        [SwaggerResponse(400, "Document is not found", typeof(void))]
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
        [SwaggerOperation(
            Summary = "Update new endorsement document. After endorsement document is updated, process is started immediately.",
            Tags = new[] { "Endorsement Document" }
        )]
        [Route("update")]
        [HttpPut]
        [SwaggerResponse(201, "Success, endorsement document is updated successfully", typeof(List<UpdateDocumentCommandDto>))]
        [SwaggerResponse(400, "Document is not found", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateDocumentAsync([FromBody] UpdateDocumentCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }
        #endregion

        #region Belge Detay
        [Route("detail")]
        [HttpGet]
        [SwaggerResponse(200, "Success, document detail is returned successfully.", typeof(GetDocumentDetailsDto))]
        [SwaggerResponse(404, "document detail is not found.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetDocumentDetailAsync([FromBody] int Id)
        {
            await Mediator.Send(new GetDocumentDetailsQuery() { Id = Id });
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
        [SwaggerOperation(
            Summary = "Delete new endorsement document. After endorsement document is deleted, process is started immediately.",
            Tags = new[] { "Endorsement Document" }
        )]
        [Route("delete")]
        [HttpDelete]
        [SwaggerResponse(201, "Success, endorsement document is deleted successfully", typeof(List<DeleteDocumentCommandDto>))]
        [SwaggerResponse(400, "Document is not found", typeof(void))]
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
