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
        try
        {

            using (var client = new HttpClient())
            {

                Log.Information("Login-SSO-url {url} ", StaticValues.Authority);

                // client.BaseAddress = new Uri(StaticValues.Authority);
                //     var requestData = new
                // {
                //     client_id = StaticValues.ClientId,
                //     client_secret = StaticValues.ClientSecret,
                //     grant_type = "authorization_code",
                //     code = code,
                //     code_challange = "",
                //     scopes = new[] { "openid", "profile" }
                // };
                // string json = JsonSerializer.Serialize(requestData);
                // Log.Information("Login-SSO Result json: {json} " + json);

                // var content = new StringContent(json, Encoding.UTF8, "application/json");
                var json = @$"{{
                        ""client_id"": ""{StaticValues.ClientId}"",
                        ""client_secret"": ""{StaticValues.ClientSecret}"",
                        ""grant_type"": ""authorization_code"",
                        ""code"": ""{code}"",
                        ""code_challange"": """",
                        ""scopes"": [""openid"", ""profile""]
                    }}";

                Log.Information("Login-SSO Result json: {json} " + json);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var result = await client.PostAsync(StaticValues.Authority+"/token", content);

                if (result.IsSuccessStatusCode)
                {
                    responseContent = await result.Content.ReadAsStringAsync();
                    Log.Information("Login-SSO Response: {responseContent}", responseContent);
                }
                else
                {
                    Log.Error("Login-SSO Failed with status: {statusCode}", result.StatusCode);
                }

                Log.Information("Login-SSO Result: {responseContent} " + responseContent);
                var token = JsonSerializer.Deserialize<AuthTokenResponse>(responseContent);

                responseContent = token.AccessToken;
                // accessToken = token.Access_token;
                Log.Information("Login-SSOToken2: " + token.AccessToken);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
        }
        return responseContent;
    }

    // public async Task<AccessToken> AccessTokenResource(string accessToken)
    // {
    //     var response = new AccessToken();

    //         using (var client = new HttpClient())
    //         {
    //             client.BaseAddress = new Uri(StaticValues.ApiGateway);
    //             var content = new FormUrlEncodedContent(new[]
    //             {
    //                     new KeyValuePair<string, string>("access_token", accessToken),
    //             });
    //             var result = await client.PostAsync("/ib/Resource", content);
    //             var responseContent = result.Content.ReadAsStringAsync().Result;
    //             response = JsonConvert.DeserializeObject<AccessToken>(responseContent);
    //             Log.Information("Login-SSO: " + responseContent);
    //             if (!string.IsNullOrEmpty(response.CitizenshipNumber))
    //                 response.IsLogin = true;


    //         }

    //     return response;
    // }
}
