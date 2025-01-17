// using Application.Common.Interfaces;
// using Infrastructure.SsoServices;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Filters;
// using Newtonsoft.Json;
// using Serilog;
// using System.Collections.Generic;
// using System.Linq;
// using System.Security.Claims;
// using System.Threading.Tasks;

// [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
// public class AuthorizeUserAttribute : Attribute, IAsyncAuthorizationFilter
// {
//     private readonly string _authenticationScheme;

//     public AuthorizeUserAttribute(string authenticationScheme)
//     {
//         _authenticationScheme = authenticationScheme;
//     }

//     public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
//     {
//         const string logPrefix = "OnAuthorizationLogs";
//         Log.Information("{LogPrefix} - Authorization started", logPrefix);

//         try
//         {
//             // Extract Authorization header
//             string accessToken = "";
//             string authorizationHeader = context?.HttpContext?.Request?.Headers["Authorization"];
//             Log.Information("{LogPrefix} - Authorization header value: {Header}", logPrefix, authorizationHeader);

//             if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
//             {
//                 accessToken = authorizationHeader.Substring("Bearer ".Length);
//                 Log.Information("{LogPrefix} - Extracted access token: {Token}", logPrefix, accessToken);
//             }
//             else
//             {
//                 Log.Warning("{LogPrefix} - Authorization header is invalid or missing", logPrefix);
//             }

//             // Resolve UserService
//             var userService = context.HttpContext.RequestServices.GetService<IUserService>();
//             if (userService == null)
//             {
//                 Log.Error("{LogPrefix} - UserService could not be resolved", logPrefix);
//                 context.Result = new UnauthorizedResult();
//                 return;
//             }

//             // Fetch user data using access token
//             Log.Information("{LogPrefix} - Fetching user data from AccessTokenResource", logPrefix);
//             var code = context.HttpContext.Request.Query["code"].ToString();
//             Log.Information("{LogPrefix} Code :{code}", logPrefix,code);

//             if (string.IsNullOrEmpty(code))
//             {
//                 throw new ArgumentException("Endorsement Login Code :Authorization code is missing or invalid.");
//             }
//             var response = await userService.AccessToken(code);


//             if (response == null || string.IsNullOrEmpty(response.CitizenshipNumber))
//             {
//                 Log.Warning("{LogPrefix} - Missing R-User-Name header", logPrefix);

//                 var userNameHeader = context.HttpContext.Request.Headers["R-User-Name"].FirstOrDefault() ?? string.Empty;
//                 Log.Warning("{LogPrefix} - Missing R-User-Name {userNameHeader}", logPrefix, userNameHeader);

//                 if (string.IsNullOrEmpty(userNameHeader))
//                 {
//                     var ssoIntegrationService = context.HttpContext.RequestServices.GetService<ISSOIntegrationService>();
//                     Log.Warning("{LogPrefix} - ssoIntegrationService start ", logPrefix, userNameHeader);

//                     var citizenship = context.HttpContext.Request.Headers["User-Reference"].FirstOrDefault() ?? string.Empty;
//                     Log.Warning("{LogPrefix} - ssoIntegrationService {citizenship} ", logPrefix, citizenship);

//                     if (string.IsNullOrEmpty(citizenship)) return;

//                     var getCustomerByCitizenshipNo = ssoIntegrationService.GetCustomerByCitizenshipNo(citizenship).Result;
//                     if (getCustomerByCitizenshipNo.Data.Length < 2)
//                     {
//                         return;
//                     }
//                     Log.Warning("{LogPrefix} - ssoIntegrationService getCustomerByCitizenshipNo  {getCustomerByCitizenshipNo.Data } ", logPrefix, getCustomerByCitizenshipNo.Data);

//                     var getUserInfoByCustomerNo = ssoIntegrationService.GetUserInfoByCustomerNo(getCustomerByCitizenshipNo.Data.ToString()).Result;
//                     if (getUserInfoByCustomerNo.Data == null)
//                     {
//                         return;
//                     }
//                     Log.Warning("{LogPrefix} - ssoIntegrationService getUserInfoByCustomerNo  {getUserInfoByCustomerNo.Data } ", logPrefix, getUserInfoByCustomerNo.Data);

//                     if (getUserInfoByCustomerNo.Data.IndexOf('\\') != -1)
//                     {
//                         var parts = getUserInfoByCustomerNo.Data.Split('\\');
//                         userNameHeader = (parts != null && parts.Length > 1) ? parts[1] : string.Empty;
//                     }
//                 }
//                 Log.Warning("{LogPrefix} - AccessTokenResource returned no user data for token: {userNameHeader}", logPrefix, userNameHeader);


//                 var claims2 = new List<Claim>
//                         {
//                             new Claim("username", userNameHeader),
//                         };
//                 var identity2 = new ClaimsIdentity(claims2, _authenticationScheme);
//                 var principal2 = new ClaimsPrincipal(identity2);
//                 context.HttpContext.User = principal2;
//                 Api.Extensions.ClaimsPrincipalExtensions.IsCredentials(principal2, userNameHeader);
//                 context.HttpContext.User = principal2;
//                 Log.Warning("{LogPrefix} - AccessTokenResource returned amorphie end token: {userNameHeader}", logPrefix, userNameHeader);
//                 return;
//             }

//             Log.Information("{LogPrefix} - User data fetched successfully: {Response}", logPrefix, JsonConvert.SerializeObject(response));
//             var claims = new List<Claim>
//             {
//                 new Claim("CitizenshipNumber", response.CitizenshipNumber ?? ""),
//                 new Claim("CustomerNumber", response.CustomerNumber ?? ""),
//                 new Claim("FirstName", response.FirstName ?? ""),
//                 new Claim("LastName", response.LastName ?? ""),
//                 new Claim("IsStaff", response.IsStaff ?? ""),
//                 new Claim("BranchCode", response.BranchCode ?? ""),
//                 new Claim("BusinessLine", response.BusinessLine ?? ""),
//                 new Claim("IsLogin", response.IsLogin.ToString()?? ""),
//                 new Claim("username", response.CitizenshipNumber?? ""),
//                 new Claim("customer_number", response.CustomerNumber?? ""),
//                 new Claim("given_name",response.FirstName?? ""),
//                 new Claim("branch_id", response.BranchCode?? ""),
//                 new Claim("family_name", response.LastName?? ""),
//                 new Claim("business_line", response.BusinessLine?? ""),
//             };
//             // Create claims

//             Log.Information("{LogPrefix} - Core claims added: {Claims}", logPrefix, claims.Select(c => c.Type));

//             // Add credentials to claims
//             if (response.Credentials != null && response.Credentials.Any())
//             {
//                 foreach (var credential in response.Credentials)
//                 {
//                     claims.Add(new Claim("credentials", credential));
//                 }

//                 Log.Information("{LogPrefix} - Credentials added to claims: {Credentials}", logPrefix, response.Credentials);
//             }

//             // Create identity and principal
//             var identity = new ClaimsIdentity(claims, _authenticationScheme);
//             var principal = new ClaimsPrincipal(identity);
//             context.HttpContext.User = principal;

//             Log.Information("{LogPrefix} - Claims successfully added to HttpContext", logPrefix);

//             // Validate user presence
//             if (context.HttpContext.User == null)
//             {
//                 Log.Error("{LogPrefix} - User is null after setting claims", logPrefix);
//             }
//             else
//             {
//                 var userClaims = context.HttpContext.User.Claims.Select(c => new { c.Type, c.Value }).ToList();
//                 Log.Information("{LogPrefix} - User claims: {Claims}", logPrefix, JsonConvert.SerializeObject(userClaims));
//             }

//         }
//         catch (Exception ex)
//         {
//             Log.Error(ex, "{LogPrefix} - Exception occurred during authorization", logPrefix);
//             context.Result = new UnauthorizedResult();
//         }

//         Log.Information("{LogPrefix} - Authorization process ended", logPrefix);
//     }
// }
