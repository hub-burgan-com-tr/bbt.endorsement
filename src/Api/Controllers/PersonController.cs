using Api.Extensions;
using Application.BbtInternals.Queries.GetCustomerSearchs;
using Application.BbtInternals.Queries.GetPersonSummary;
using Application.BbtInternals.Queries.GetSearchPersonSummary;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Api.Controllers
{
   // [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("")]
    [ApiController]
    public class PersonController : ApiControllerBase
    {
        [Route("GetUserInfo")]
        [HttpGet]
        //[SwaggerResponse(200, "Success, queried user search are returned successfully.", typeof(GetSearchPersonSummaryDto))]
        //[SwaggerResponse(404, "Success but there is no user search  available for the query.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public GetSearchPersonSummaryDto GetUserInfo()
        {

            var claims = HttpContext.User.Claims;
            // var token  = string.Empty;
            // if (HttpContext.Request.Headers.ContainsKey("Authorization"))
            // {
            //     var value = HttpContext.Request.Headers["Authorization"].ToString();
            //     Log.Information("GetUserInfo {Key}: {Value}", "Authorization", value);
            //     token = value.Replace("Bearer","").Trim();
            // }
            // else
            // {
            //     Log.Information("GetUserInfo {Key} header not found.", "Authorization");
            // }
            // DTO oluşturuluyor
            var dto = new GetSearchPersonSummaryDto
            {
                // Token =token,
                First = claims.FirstOrDefault(c => c.Type == "given_name")?.Value,
                Last = claims.FirstOrDefault(c => c.Type == "family_name")?.Value,
                CustomerNumber = UInt64.TryParse(claims.FirstOrDefault(c => c.Type == "customer_number")?.Value, out var customerNumber)
                                 ? customerNumber : 0,
                BusinessLine = claims.FirstOrDefault(c => c.Type == "business_line")?.Value,
            };
            var credentials = claims.FirstOrDefault(c => c.Type == "credentials")?.Value;
            if (!string.IsNullOrEmpty(credentials))
            {
                var credentialsList = credentials.Split(',').ToList();  // credentials'ı ',' ile ayırıp listeye çeviriyoruz
                GetCredentials(dto, credentialsList);  // Listeyi metota gönderiyoruz
            }
            Log.Information("Login-Start GetSearchPersonSummaryDto: {dto} ", JsonConvert.SerializeObject(dto));

            return dto;
        }
        private void GetCredentials(GetSearchPersonSummaryDto result, List<string> Credentials)
        {


            foreach (var credential in Credentials)
            {
                var value = credential.Split("###");
                if (value.Length == 2)
                {
                    bool isActive = value[1] == "1"; // 1 olduğunda aktif, diğer durumda pasif

                    switch (value[0])
                    {
                        case "isFormReader":
                            result.Authory.IsFormReader = isActive;
                            break;
                        case "isNewFormCreator":
                            result.Authory.IsNewFormCreator = isActive;
                            break;
                        case "isReadyFormCreator":
                            result.Authory.IsReadyFormCreator = isActive;
                            break;
                        case "isBranchApproval":
                            result.Authory.IsBranchApproval = isActive;
                            break;
                        case "isBranchFormReader":
                            result.Authory.IsBranchFormReader = isActive;
                            break;
                        case "isUIVisible":
                            result.Authory.isUIVisible = isActive;
                            break;
                    }
                }
            }
        }
        [Route("CustomerSearch")]
        [HttpGet]
        [SwaggerResponse(200, "Success, queried person search are returned successfully.", typeof(GetSearchPersonSummaryDto))]
        [SwaggerResponse(404, "Success but there is no person search  available for the query.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CustomerSearch([FromQuery] string name)
        {
            var person = UserExtensions.GetOrderPerson(User.Claims);
            var response = await Mediator.Send(new GetCustomerSearchQuery { Name = name, Person = person });
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
            var person = UserExtensions.GetOrderPerson(User.Claims);
            var response = await Mediator.Send(new GetSearchPersonSummaryQuery() { Name = name, Person = person });
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
