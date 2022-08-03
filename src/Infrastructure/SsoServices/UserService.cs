﻿using Application.Common.Models;
using Infrastructure.SsoServices.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;

namespace Infrastructure.SsoServices;
public interface IUserService
{
    Task<AccessToken> AccessToken(string code, string state);
    Task<AccessToken> AccessTokenResource(string accessToken);
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
                        new KeyValuePair<string, string>("client_id", "Endorsement"),
                        new KeyValuePair<string, string>("grant_type", "authorization_code"),
                        new KeyValuePair<string, string>("client_secret", StaticValues.ClientSecret),
                        new KeyValuePair<string, string>("redirect_uri", StaticValues.RedirectUri),
                    });
                    var result = await client.PostAsync("/connect/token", content);
                    var responseContent = result.Content.ReadAsStringAsync().Result;
                    var token = JsonConvert.DeserializeObject<AccessToken>(responseContent);
                    accessToken = token.Access_token;
                    Log.Information("Login-SSOToken: " + accessToken);
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
                    Log.Information("Login-SSO: " + responseContent);
                    if (!string.IsNullOrEmpty(response.CitizenshipNumber))
                        response.IsLogin = true;
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
        }
        return response;
    }

    public async Task<AccessToken> AccessTokenResource(string accessToken)
    {
        var response = new AccessToken();

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
                Log.Information("Login-SSO: " + responseContent);
                if (!string.IsNullOrEmpty(response.CitizenshipNumber))
                    response.IsLogin = true;

                //if (response.IsLogin)
                //{
                //    _httpContextAccessor.HttpContext.Session.SetString("CustomerNumber", response.CustomerNumber != null ? response.CustomerNumber : "");
                //    _httpContextAccessor.HttpContext.Session.SetString("CitizenshipNumber", response.CitizenshipNumber != null ? response.CitizenshipNumber : "");
                //    _httpContextAccessor.HttpContext.Session.SetString("FirstName", response.FirstName != null ? response.FirstName : "");
                //    _httpContextAccessor.HttpContext.Session.SetString("LastName", response.LastName != null ? response.LastName : "");
                //    _httpContextAccessor.HttpContext.Session.SetString("BusinessLine", response.BusinessLine != null ? response.BusinessLine : "");
                //    _httpContextAccessor.HttpContext.Session.SetString("BranchCode", response.BranchCode != null ? response.BranchCode : "");
                //    _httpContextAccessor.HttpContext.Session.SetString("IsStaff", response.IsStaff != null ? response.IsStaff : "");

                //    if (response.Credentials != null)
                //    {
                //        foreach (var credential in response.Credentials)
                //        {
                //             var value = credential.Split("###");
                //            _httpContextAccessor.HttpContext.Session.SetString(value[0].ToLower(), value[1]);
                //        }
                //    }
                //}
            }
        
        return response;
    }
}
