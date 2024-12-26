using Application.Common.Models;
using Microsoft.AspNetCore.Http;
using System.Net;
using Serilog;


namespace Infrastructure.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    static readonly ILogger Log = Serilog.Log.ForContext<ErrorHandlerMiddleware>();

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
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
        response.StatusCode = 417;
        var message = "Endorsement Error: ";
        if (ex.Message is not null)
            message += ex.Message.ToString();

        var result = Response<object>.Fail(message, response.StatusCode);
        Log.Error(ex, $"{DateTime.UtcNow.ToString("HH:mm:ss")} : {message}");

        return response.WriteAsync(result.ToString());
    }
}