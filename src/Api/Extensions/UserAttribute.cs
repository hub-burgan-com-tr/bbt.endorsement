using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Common.Interfaces;
using Infrastructure.SsoServices;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

public class UserAttribute : Attribute, IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
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

        var customerNo = string.Empty;
        // customer_no başlığını almak
        if (headers.ContainsKey("customer_no"))
        {
            customerNo = headers["customer_no"].ToString();

            // customer_no değerini logla
            Log.Information("Customer No: {CustomerNo}", customerNo);
        }
        else
        {
            Log.Information("Customer No header not found.");
        }
        var claims2 = new List<Claim>
                        {
                            new Claim("username", "f"),
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
}