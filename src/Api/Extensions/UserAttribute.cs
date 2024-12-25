using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.BbtInternals.Queries.GetSearchPersonSummary;
using Application.Common.Interfaces;
using Infrastructure.SsoServices;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Serilog;

public class UserAttribute : Attribute, IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
        var controllerName = context.RouteData.Values["controller"]?.ToString();
        if (controllerName == "Home")
        {
            Log.Information("HomeController is excluded from UserAttribute");
            return;
        }
        Log.Information("OnActionExecutedStart");

        // Başlıkları al
        var headers = context.HttpContext.Request.Headers;

        // Header'ları bir dictionary'e ekleyelim
        var headersDict = new Dictionary<string, IEnumerable<string>>();

        foreach (var header in headers)
        {
            headersDict.Add(header.Key, header.Value);
        }

        // Dictionary'i JSON'a dönüştür
        var headersJson = JsonSerializer.Serialize(headersDict);

        // JSON formatındaki header bilgisini logla
        Log.Information("Request Headers: {Headers}", headersJson);
        Log.Information("OnActionExecutedEnd");

        var customerNo = GetHeaderValue(headers, "customer_no");
        var isbankpersonel = Convert.ToBoolean(GetHeaderValue(headers, "isbankpersonel", "false"));
        var user_reference = GetHeaderValue(headers, "user_reference");
        var given_name = GetHeaderValue(headers, "given_name");
        var family_name = GetHeaderValue(headers, "family_name");

        if (!isbankpersonel)
        {
            var result = new GetSearchPersonSummaryDto
            {
                CitizenshipNumber = user_reference,
                IsStaff = false,
                CustomerNumber = Convert.ToUInt64(customerNo),
                First = given_name,
                Last = family_name,
                BusinessLine = "B",
            };
        }
        var claims2 = new List<Claim>
                        {
                            new Claim("username", user_reference),
                            new Claim("customer_number", customerNo),
                            new Claim("given_name", given_name),
                            new Claim("family_name", family_name),
                            new Claim("business_line", "B"),
                            new Claim("credentials", "isBranchFormReader###1")
                        };
        var identity2 = new ClaimsIdentity(claims2);
        var principal2 = new ClaimsPrincipal(identity2);
        context.HttpContext.User = principal2;
        Api.Extensions.ClaimsPrincipalExtensions.IsCredentials(principal2, customerNo);


    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        Log.Information("OnActionExecutingStart2");
        Log.Information("OnActionExecutingEnd2");

    }
    private string GetHeaderValue(IHeaderDictionary headers, string key, string defaultValue = "")
    {
        if (headers.ContainsKey(key))
        {
            var value = headers[key].ToString();
            Log.Information("{Key}: {Value}", key, value);
            return value;
        }
        else
        {
            Log.Information("{Key} header not found.", key);
            return defaultValue;
        }
    }

}