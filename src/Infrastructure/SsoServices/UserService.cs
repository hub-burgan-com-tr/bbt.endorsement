using Application.Common.Models;
using Infrastructure.SsoServices.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SsoServices;
public interface IUserService
{
    Task<AccessToken> AccessToken(string code, string state);
}

public class UserService : IUserService
{
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
                        new KeyValuePair<string, string>("client_id", "Endorsement"),
                        new KeyValuePair<string, string>("grant_type", "authorization_code"),
                        new KeyValuePair<string, string>("client_secret", "A615C904-4E98-4153-8C53-B9174D4FD32B"),
                        new KeyValuePair<string, string>("redirect_uri", StaticValues.RedirectUri),
                    });
                    var result = await client.PostAsync("/connect/token", content);
                    var responseContent = result.Content.ReadAsStringAsync().Result;
                    var token = JsonConvert.DeserializeObject<AccessToken>(responseContent);
                    accessToken = token.access_token;
                }

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.ApiGateway);
                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("access_token", accessToken),
                    });
                    var result = await client.PostAsync("/ib/Resource", content);
                    var responseContent = result.Content.ReadAsStringAsync().Result;
                    response = JsonConvert.DeserializeObject<AccessToken>(responseContent);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex.InnerException;
        }
        return response;
    }
}
