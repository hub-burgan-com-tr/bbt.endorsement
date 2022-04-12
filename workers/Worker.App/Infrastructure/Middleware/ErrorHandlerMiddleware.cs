using Microsoft.AspNetCore.Http;
using Serilog;
using System.Net;
using Worker.App.Application.Common.Models;

namespace Worker.App.Infrastructure.Middleware;
public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var response = context.Response;

        response.ContentType = "application/json";
        response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var message = "";
        if (ex.Message is not null)
            message = ex.Message.ToString();

        var result = Response<object>.Fail(message, response.StatusCode);
        Log.Error(ex, $"{DateTime.UtcNow.ToString("HH:mm:ss")} : {message}");

        return response.WriteAsync(result.ToString());
    }
}
