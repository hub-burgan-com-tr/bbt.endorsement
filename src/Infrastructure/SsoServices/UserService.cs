using System.Text;
using System.Text.Json;
using Application.Common.Models;
using Infrastructure.SsoServices.Models;
using Microsoft.AspNetCore.Http;

using Serilog;

namespace Infrastructure.SsoServices;
public interface IUserService
{
    Task<AccessToken> AccessToken(string code, string state);
    // Task<AccessToken> AccessTokenResource(string accessToken);
}

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AccessToken> AccessToken(string code, string state)
    {
        string accessToken = "";
        var response = new AccessToken();
        try
        {
            if (state == "EndorsementGondor")
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.Authority);
                    var requestData = new
                    {
                        client_id = StaticValues.ClientId,
                        client_secret = StaticValues.ClientSecret,
                        grant_type = "authorization_code",
                        code = code,
                        code_challenge = "",
                        scopes = new[] { "openid", "profile" }
                    };
                    string json = JsonSerializer.Serialize(requestData);

                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var result = await client.PostAsync("/token", content);

                    var responseContent = result.Content.ReadAsStringAsync().Result;

                    Log.Information("Login-SSO Result: {responseContent} " + responseContent);

                    var token = JsonSerializer.Deserialize<AccessToken>(responseContent);
                    accessToken = token.Access_token;
                    Log.Information("Login-SSOToken2: " + accessToken);
                }

                // using (var client = new HttpClient())
                // {
                //     client.BaseAddress = new Uri(StaticValues.ApiGateway);
                //     var content = new FormUrlEncodedContent(new[]
                //     {
                //         new KeyValuePair<string, string>("access_token", accessToken),
                //     });
                //     var result = await client.PostAsync("/ib/Resource", content);
                //     var responseContent = result.Content.ReadAsStringAsync().Result;
                //     response = JsonConvert.DeserializeObject<AccessToken>(responseContent);
                //     Log.Information("Login-SSO: " + responseContent);
                //     if (!string.IsNullOrEmpty(response.CitizenshipNumber))
                //         response.IsLogin = true;
                // }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
        }
        return response;
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
