using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

public class UserAttribute :Attribute, IActionFilter
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

    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        Log.Information("OnActionExecutingStart2");
        Log.Information("OnActionExecutingEnd2");
 
    }
}