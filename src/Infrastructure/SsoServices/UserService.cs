using Application.Common.Models;
using Infrastructure.SsoServices.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
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
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("code", code),
                        new KeyValuePair<string, string>("client_id", StaticValues.ClientId),
                        new KeyValuePair<string, string>("grant_type", "authorization_code"),
                        new KeyValuePair<string, string>("client_secret", StaticValues.ClientSecret),
                        new KeyValuePair<string, string>("redirect_uri", StaticValues.RedirectUri),
                    });
                    var result = await client.PostAsync("/token", content);

                    var responseContent = result.Content.ReadAsStringAsync().Result;

                    Log.Information("Login-SSO Result: {responseContent} " + responseContent);

                    var token = JsonConvert.DeserializeObject<AccessToken>(responseContent);
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
