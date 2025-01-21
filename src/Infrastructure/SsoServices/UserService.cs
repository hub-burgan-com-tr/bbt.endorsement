using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Application.Common.Models;
using Infrastructure.SsoServices.Models;
using Microsoft.AspNetCore.Http;

using Serilog;

namespace Infrastructure.SsoServices;
public interface IUserService
{
    Task<string> AccessToken(string code);
    // Task<AccessToken> AccessTokenResource(string accessToken);
}

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> AccessToken(string code)
    {
        var responseContent = string.Empty;
        var responseToken = string.Empty;
        try
        {

            var client = new HttpClient();



            var json = @$"{{
                        ""client_id"": ""{StaticValues.ClientId}"",
                        ""client_secret"": ""{StaticValues.ClientSecret}"",
                        ""grant_type"": ""authorization_code"",
                        ""code"": ""{code}"",
                        ""code_challange"": """",
                        ""scopes"": [""openid"", ""profile""]
                    }}";

            Log.Information("Login-SSO Result json: {AccessTokenRequest} " + json);
            var request = new HttpRequestMessage(HttpMethod.Post, StaticValues.Authority + "/token");
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Content = content;
            var result = await client.SendAsync(request);

            if (result.IsSuccessStatusCode)
            {
                responseContent = await result.Content.ReadAsStringAsync();
            }
            else
            {
                Log.Error("Login-SSO Failed with status: {statusCode}", result.StatusCode);
            }

            var token = JsonSerializer.Deserialize<AuthTokenResponse>(responseContent);

            responseToken = token.AccessToken;
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
        }
        return responseToken;
    }

}
