using System.Text.Json;
using System.Text.Json.Serialization;

namespace Worker.App.Application.Common.Models;


public class Response<T>
{
    public T Data { get; set; }

    [JsonIgnore]
    public int StatusCode { get; set; }

    public string Message { get; set; }

    public static Response<T> Success(T data, int statusCode)
    {
        return new Response<T> { Data = data, StatusCode = statusCode };
    }

    public static Response<T> Success(int statusCode)
    {
        return new Response<T> { Data = default(T), StatusCode = statusCode };
    }

    public static Response<T> Fail(string error, int statusCode)
    {
        return new Response<T>
        {
            Message = error,
            StatusCode = statusCode
        };
    }

    public static Response<T> NotFoundException(string error, int statusCode)
    {
        return new Response<T>
        {
            Message = error,
            StatusCode = statusCode
        };
    }

    public static Response<T> NotFoundException(string name, object key, int statusCode)
    {
        return new Response<T>
        {
            Message = $"Entity \"{name}\" ({key}) was not found.",
            StatusCode = statusCode
        };
    }


    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
