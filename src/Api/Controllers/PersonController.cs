using Api.Extensions;
using Application.BbtInternals.Queries.GetCustomerSearchs;
using Application.BbtInternals.Queries.GetPersonSummary;
using Application.BbtInternals.Queries.GetSearchPersonSummary;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Text.Json;

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
            var token  = string.Empty;
            if (HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                var value = HttpContext.Request.Headers["Authorization"].ToString();
                Log.Information("GetUserInfo {Key}: {Value}", "Authorization", value);
                token = value.Replace("Bearer","").Trim();
            }
            else
            {
                Log.Information("GetUserInfo {Key} header not found.", "Authorization");
            }
            // DTO oluşturuluyor
            //  new Claim("username", user_reference),
            //             new Claim("customer_number", customerNo),
            //             new Claim("given_name", given_name),
            //             new Claim("family_name", family_name),
            //             new Claim("business_line", "B"),
            //             new Claim("credentials", "isBranchFormReader###1")
            var dto = new GetSearchPersonSummaryDto
            {
                Token = token,
                CitizenshipNumber = claims.FirstOrDefault(c => c.Type == "user_reference")?.Value ?? "",
                First = claims.FirstOrDefault(c => c.Type == "given_name")?.Value ?? "",
                Last = claims.FirstOrDefault(c => c.Type == "family_name")?.Value ?? "",
                CustomerNumber = UInt64.TryParse(claims.FirstOrDefault(c => c.Type == "customer_number")?.Value, out var customerNumber)
                                 ? customerNumber : 0,
                BusinessLine = claims.FirstOrDefault(c => c.Type == "business_line")?.Value ?? "B",
            };
            var credentials = claims.Where(c => c.Type == "credentials").Select(x => x.Value).ToList();
            GetCredentials(dto, credentials);


            Log.Information("Login-Start GetSearchPersonSummaryDto: {dto} ", JsonSerializer.Serialize(dto));

            return dto;
        }
        private void GetCredentials(GetSearchPersonSummaryDto result, List<string> Credentials)
        {
            var authory = result.Authory;

            foreach (var credential in Credentials)
            {
                var value = credential.Split("###");

                if (value.Length == 2)
                {
                    bool isActive = value[1] == "1";
                    var claimMapping = new Dictionary<string, Action<bool>>
                        {
                            { "isFormReader", (status) => authory.IsFormReader = status },
                            { "isNewFormCreator", (status) => authory.IsNewFormCreator = status },
                            { "isReadyFormCreator", (status) => authory.IsReadyFormCreator = status },
                            { "isBranchApproval", (status) => authory.IsBranchApproval = status },
                            { "isBranchFormReader", (status) => authory.IsBranchFormReader = status },
                            { "isUIVisible", (status) => authory.isUIVisible = status }
                        };

                    // Eğer dictionary'de bu credential varsa, aktif/pasif değerini atıyoruz.
                    if (claimMapping.ContainsKey(value[0]))
                    {
                        claimMapping[value[0]](isActive);
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
