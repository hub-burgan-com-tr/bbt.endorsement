using Application.SSOIntegrationService.Commands;
using Domain.Models;
using Google.Protobuf.WellKnownTypes;
using Infrastructure.SSOIntegration;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Serilog;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static long GetCitizenshipNumber(this ClaimsPrincipal principal)
    {
        var citizenshipNumber = long.Parse(principal.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString());

        return citizenshipNumber;
    }
    public static bool IsCredentials(this ClaimsPrincipal principal, string requestUserName)
    {
        try
        {
            if (!string.IsNullOrEmpty(requestUserName) && !principal.Claims.Any(c => c.Type == "credentials"))
            {
                Log.Information("GetSSOClaims start " + requestUserName);

                var sync = GetSSOClaims(principal, requestUserName).Result;
            }
        }
        catch (Exception e)
        {
            Log.Warning("GetSSOClaims Hata Alindi UserName= " + requestUserName + " " + e.Message);
        }

        var credentials = principal.Claims.Where(c => c.Type == "credentials").ToList();

        return credentials.Count > 0;
    }

    private static async Task<ClaimsPrincipal> GetSSOClaims(ClaimsPrincipal principal, string requestUserName)
    {
        var person = new OrderPerson();

        if (string.IsNullOrEmpty(requestUserName))
        {
            return null;
        }
        if (requestUserName.Length < 4)
        {
            return null;
        }
        var res = new SSOIntegrationResponse();
        var ssoService = new SSOIntegrationService();

        var responseRegisterId = await ssoService.SearchUserInfo(requestUserName);
        Log.Information("SSOResponseMapClaims " + responseRegisterId);
        if (responseRegisterId.StatusCode == 200)
        {
            res.RegisterId = responseRegisterId.Data;
            Log.Information(" responseRegisterId.Data " + responseRegisterId.Data);

            var resUserByRegisterId = await ssoService.GetUserByRegisterId(res.RegisterId);
            if (resUserByRegisterId.StatusCode == 200)
            {
                var resAuthorityForUser = await ssoService.GetAuthorityForUser("MOBIL_ONAY", "Credentials", res.RegisterId);
                Log.Information("resAuthorityForUser.Data " + resAuthorityForUser.Data);
                res.UserAuthorities = resAuthorityForUser.Data;
            }
        }

        return SSOResponseMapClaims(principal, res);
    }
    private static ClaimsPrincipal SSOResponseMapClaims(ClaimsPrincipal principal, SSOIntegrationResponse ssoResponse)
    {
        var identity = principal.Identity as ClaimsIdentity;
        identity.AddClaim(new Claim("username", ssoResponse.UserInfo.CitizenshipNumber));
        identity.AddClaim(new Claim("customer_number", ssoResponse.UserInfo.CustomerNo));
        identity.AddClaim(new Claim("given_name", ssoResponse.UserInfo.FirstName));
        identity.AddClaim(new Claim("family_name", ssoResponse.UserInfo.Surname));
        identity.AddClaim(new Claim("branch_id", ssoResponse.UserInfo.BranchCode));
        identity.AddClaim(new Claim("email", ssoResponse.UserInfo.Email));
        identity.AddClaim(new Claim("family_name", ssoResponse.UserInfo.Surname));
        identity.AddClaim(new Claim("credentials", ssoResponse.UserAuthorities.Where(x => x.Name == "isBranchFormReader").Select(x => x.Name + "###" + x.Value).FirstOrDefault()));
        identity.AddClaim(new Claim("credentials", ssoResponse.UserAuthorities.Where(x => x.Name == "isBranchApproval").Select(x => x.Name + "###" + x.Value).FirstOrDefault()));
        identity.AddClaim(new Claim("credentials", ssoResponse.UserAuthorities.Where(x => x.Name == "isReadyFormCreator").Select(x => x.Name + "###" + x.Value).FirstOrDefault()));
        identity.AddClaim(new Claim("credentials", ssoResponse.UserAuthorities.Where(x => x.Name == "isNewFormCreator").Select(x => x.Name + "###" + x.Value).FirstOrDefault()));
        identity.AddClaim(new Claim("credentials", ssoResponse.UserAuthorities.Where(x => x.Name == "isFormReader").Select(x => x.Name + "###" + x.Value).FirstOrDefault()));
        identity.AddClaim(new Claim("credentials", ssoResponse.UserAuthorities.Where(x => x.Name == "isUIVisible").Select(x => x.Name + "###" + x.Value).FirstOrDefault()));
        Log.Information("SSOResponseMapClaims " + ssoResponse);
        return principal;
    }


    #region func
    public static string GetUserEmail(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.Email);
    }

    public static string GetUserId(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public static string GetUserName(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.Name);
    }

    public static bool IsCurrentUser(this ClaimsPrincipal principal, string id)
    {
        var currentUserId = GetUserId(principal);

        return string.Equals(currentUserId, id, StringComparison.OrdinalIgnoreCase);
    }
    #endregion

}