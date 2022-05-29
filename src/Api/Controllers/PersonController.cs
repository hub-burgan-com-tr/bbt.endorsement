using Application.BbtInternals.Models;
using Application.BbtInternals.Queries.GetCustomers;
using Application.BbtInternals.Queries.GetPersonSummary;
using Application.BbtInternals.Queries.GetSearchPersonSummary;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PersonController : ApiControllerBase
    {
        [Route("customer-search")]
        [HttpPost]
        [SwaggerResponse(200, "Success, queried person search are returned successfully.", typeof(GetSearchPersonSummaryDto))]
        [SwaggerResponse(404, "Success but there is no person search  available for the query.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CustomerSearch([FromBody] CustomerRequest customer)
        {
            var response = await Mediator.Send(new GetCustomerQuery { Customer = customer });
            return Ok(response);
        }

        [Route("person-search")]
        [HttpGet]
        [SwaggerResponse(200, "Success, queried person search are returned successfully.", typeof(GetSearchPersonSummaryDto))]
        [SwaggerResponse(404, "Success but there is no person search  available for the query.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> PersonSearch([FromQuery] string name)
        {
            var response = await Mediator.Send(new GetSearchPersonSummaryQuery() { Name = name });
            return Ok(response);
        }

        [Route("person-get")]
        [HttpGet]
        [SwaggerResponse(200, "Success, queried person get are returned successfully.", typeof(GetSearchPersonSummaryDto))]
        [SwaggerResponse(404, "Success but there is no person get  available for the query.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> PersonGet([FromQuery] long Id)
        {

            var response = await Mediator.Send(new GetPersonSummaryQuery() { Id = Id });
            return Ok(response);
        }
    }
}
