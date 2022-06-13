using Infrastructure.SsoServices.Models;
using Newtonsoft.Json;

namespace Infrastructure.SsoServices;

public interface ISsoService
{
    Task<AccessToken> AccessToken(string code, string state);
}

public class SsoService : ISsoService
{
    public async Task<AccessToken> AccessToken(string code, string state)
    {
        string accessToken = "";
        if (state == "LoyaltyGondor")
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://gondor-apigateway.burgan.com.tr");
                var content = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("code", code),
                        new KeyValuePair<string, string>("client_id", "Loyalty"),
                        new KeyValuePair<string, string>("grant_type", "authorization_code"),
                        new KeyValuePair<string, string>("client_secret", "A615C904-4E98-4153-8C53-B9174D4FD32B"),
                        new KeyValuePair<string, string>("redirect_uri", "https://localhost:44341/Home/AccessToken"),
                    });
                var result = await client.PostAsync("/connect/token", content);
                var responseContent = result.Content.ReadAsStringAsync().Result;
                var token = JsonConvert.DeserializeObject<AccessToken>(result.Content.ReadAsStringAsync().Result);
                accessToken = token.access_token;
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://gondor-apigateway.burgan.com.tr"); //
                var content = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("access_token", accessToken),
                    });
                var result = await client.PostAsync("/ib/Resource", content);
                var responseContent = result.Content.ReadAsStringAsync().Result;
            }
        }
        else if (state == "EndorsementGondor")
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://gondor-apigateway.burgan.com.tr");
                var content = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("code", code),
                        new KeyValuePair<string, string>("client_id", "Endorsement"),
                        new KeyValuePair<string, string>("grant_type", "authorization_code"),
                        new KeyValuePair<string, string>("client_secret", "A615C904-4E98-4153-8C53-B9174D4FD32B"),
                        new KeyValuePair<string, string>("redirect_uri", "https://test-bbt-endorsementui.apps.nonprod.ebt.bank/#/access-token"),
                    });
                var result = await client.PostAsync("/connect/token", content);
                var responseContent = result.Content.ReadAsStringAsync().Result;
                var token = JsonConvert.DeserializeObject<AccessToken>(result.Content.ReadAsStringAsync().Result);
                accessToken = token.access_token;
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://gondor-apigateway.burgan.com.tr");
                var content = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("access_token", accessToken),
                    });
                var result = await client.PostAsync("/ib/Resource", content);
                var responseContent = result.Content.ReadAsStringAsync().Result;
            }
        }
        var response = new AccessToken
        {
            access_token = accessToken
        };
        return response;
    }
}

