using System.Net;
using Application.Documents.Commands.CreateDocumentCommands;
using Application.Documents.Commands.DeleteDocumentCommands;
using Application.Documents.Commands.Queries.GetDocumentDetails;
using Application.Documents.Commands.Queries.GetDocuments;
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

        /// <summary>
        ///  Belge Listeleme
        /// </summary>
        /// <param name="approvalId"></param>
        /// <returns></returns>
        /// <response code="404">If the item is null</response>
        [SwaggerOperation(
            Summary = "Query endorsement approval document list.",
            Tags = new[] { "Endorsement" }
        )]
        [Route("list")]
        [HttpGet]
        [SwaggerResponse(200, "Success, approval document list is returned successfully.", typeof(List<GetDocumentsDto>))]
        [SwaggerResponse(404, "Approval document list is not found.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetDocumentAsync([FromBody] int approvalId)
        {
            await Mediator.Send(new GetDocumentsQueryDto() { ApprovalId = approvalId });
            return Ok();
        }

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
        [SwaggerResponse(201, "Success, endorsement document is created successfully", typeof(int))]
        [SwaggerResponse(400, "Document is not found", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateDocumentAsync([FromBody] CreateDocumentCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }


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
        [SwaggerResponse(201, "Success, endorsement document is updated successfully", typeof(int))]
        [SwaggerResponse(400, "Document is not found", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateDocumentAsync([FromBody] UpdateDocumentCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }

        #endregion

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
        [SwaggerResponse(201, "Success, endorsement document is deleted successfully", typeof(int))]
        [SwaggerResponse(400, "Document is not found", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteDocumentAsync([FromBody] DeleteDocumentCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }  
       
    }






    
}
