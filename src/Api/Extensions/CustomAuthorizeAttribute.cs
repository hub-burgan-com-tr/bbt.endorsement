using Infrastructure.SsoServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Services;
using Newtonsoft.Json;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
public class AuthorizeUserAttribute : Attribute, IAuthorizationFilter
{
    // public void OnActionExecuted(ActionExecutedContext context)
    // {
    //     Log.Information("OnActionExecuted Headers geldi");
    //     if (context?.HttpContext?.User?.Identity?.IsAuthenticated == true)
    //     {
    //         Log.Information("OnActionExecuted Headers IsAuthenticated");
    //     }

    //     Log.Information("OnActionExecuted Headers username" + context?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == "username").Value?.ToString());
    //     Log.Information("OnActionExecuted Headers FirstOrDefault" + context?.HttpContext?.User?.Claims?.FirstOrDefault().Value?.ToString());
    //     Log.Information("OnActionExecuted Headers LastOrDefault" + context?.HttpContext?.User?.Claims?.LastOrDefault().Value?.ToString());
    // }
    // public void OnActionExecuting(ActionExecutingContext context)
    // {
    //     Log.Information("OnActionExecuting Headers:" + JsonConvert.SerializeObject(context.HttpContext?.Request?.Headers));
    // }
    private readonly string _authenticationScheme;

    public AuthorizeUserAttribute(string authenticationScheme)
    {
        _authenticationScheme = authenticationScheme;
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var log = "OnAuthorizationLogs";
        Log.Information("OnAuthorization Headers geldi");
        string access_token = "";
        string value = context?.HttpContext?.Request?.Headers["Authorization"];
        if (value.StartsWith("Bearer "))
        {
            access_token = value.Substring("Bearer ".Length);
        }
        Log.Information(log + "-Bearer ");
        var userService = context.HttpContext.RequestServices.GetService<IUserService>();
        Log.Information(log + "-userService ");

        if (userService == null)
        {
            Log.Information(log + "userServiceif");

            context.Result = new UnauthorizedResult();
            return;
        }
        Log.Information(log + "-AccessTokenResource");

        var response = userService.AccessTokenResource(access_token).Result;
        if (response == null)
        {
            Log.Information(log + "-AccessTokenResourceifnull");
            context.Result = new UnauthorizedResult();
            return;
        }
        Log.Information(log + "-AccessTokenResourceif: {ResponseObject}", JsonConvert.SerializeObject(response));
        var claims = new List<Claim>
    {
        new Claim("CitizenshipNumber", response.CitizenshipNumber ?? ""),
        new Claim("CustomerNumber", response.CustomerNumber ?? ""),
        new Claim("FirstName", response.FirstName ?? ""),
        new Claim("LastName", response.LastName ?? ""),
        new Claim("IsStaff", response.IsStaff ?? ""),
        new Claim("BranchCode", response.BranchCode ?? ""),
        new Claim("BusinessLine", response.BusinessLine ?? ""),
        new Claim("IsLogin", response.IsLogin.ToString())
    };

        if (response.Credentials != null)
        {
            foreach (var credential in response.Credentials)
            {
                claims.Add(new Claim("Credential", credential));
            }
        }

        var identity = new ClaimsIdentity(claims, _authenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        context.HttpContext.User = principal;

        Log.Information(log + "-end");
        return;

        // if (!string.IsNullOrEmpty(response.Token))
        // {
        //     try
        //     {
        //         // Token'ı doğrula ve kullanıcıya ata
        //         var principal = ValidateToken(token);
        //         if (principal != null)
        //         {
        //             context.HttpContext.User = principal;
        //         }
        //         else
        //         {
        //             context.Result = new UnauthorizedResult();
        //         }
        //     }
        //     catch
        //     {
        //         // Token geçersiz, Unauthorized döndür
        //         context.Result = new UnauthorizedResult();
        //     }
        // }
        // else
        // {
        //     // Token bulunamadı
        //     context.Result = new UnauthorizedResult();
        // }
        // if (response.IsLogin == false)
        // {
        //     return;//token yok
        // }

        if (context?.HttpContext?.User?.Identity?.IsAuthenticated == true)
        {
            Log.Information("OnActionExecuted Headers IsAuthenticated");
            Log.Information("OnAuthorization Headers username" + context?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == "username").Value?.ToString());
            Log.Information("OnAuthorization Headers FirstOrDefault" + context?.HttpContext?.User?.Claims?.FirstOrDefault().Value?.ToString());
            Log.Information("OnAuthorization Headers LastOrDefault" + context?.HttpContext?.User?.Claims?.LastOrDefault().Value?.ToString());
        }
    }

    private ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        // Token doğrulama ayarları
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Token'ı hangi uygulamanın yayınladığını kontrol et
            ValidateAudience = true, // Token'ın hangi uygulamaya ait olduğunu kontrol et
            ValidateLifetime = true, // Token'ın süresinin geçerliliğini kontrol et
            ValidateIssuerSigningKey = true, // Token'ın doğru imzalandığını kontrol et
            ValidIssuer = "your-issuer", // Token'ı yayınlayan uygulama
            ValidAudience = "your-audience", // Token'ın kullanılacağı uygulama
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secure-key"))
        };

        SecurityToken validatedToken;
        var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
        return principal;
    }
}
// public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
// {
//     private readonly bool _useBaseAuthorizeLogic;
//     private readonly string _requiredClaim;

//     public CustomAuthorizeAttribute(string authenticationSchemes, string requiredClaim = null, bool useBaseAuthorizeLogic = true)
//     {
//         AuthenticationSchemes = authenticationSchemes;
//         _requiredClaim = requiredClaim;
//         _useBaseAuthorizeLogic = useBaseAuthorizeLogic;
//     }

//     public void OnAuthorization(AuthorizationFilterContext context)
//     {
//         // Eğer base authorize logic kullanılacaksa önce AuthorizeAttribute çalıştırılır
//         if (_useBaseAuthorizeLogic)
//         {
//             var defaultAuthorize = new AuthorizeAttribute
//             {
//                 AuthenticationSchemes = AuthenticationSchemes
//             };

//             var defaultAuthorizationFilter = new AuthorizationFilter(context.HttpContext.RequestServices, defaultAuthorize);
//             defaultAuthorizationFilter.OnAuthorization(context);

//             // Eğer mevcut yetkilendirme başarısızsa (Unauthorized/Forbidden), devam etmiyoruz
//             if (context.Result != null)
//                 return;
//         }

//         // Ek Claim kontrolü yapılır
//         if (!string.IsNullOrEmpty(_requiredClaim))
//         {
//             var user = context.HttpContext.User;

//             if (!user.Identity?.IsAuthenticated ?? true ||
//                 !user.Claims.Any(c => c.Type == _requiredClaim))
//             {
//                 context.Result = new ForbidResult();
//                 return;
//             }
//         }

//         // Ek duruma göre yetkilendirme yapılabilir
//         if (SomeAdditionalLogic(context))
//         {
//             context.Result = new ForbidResult();
//             return;
//         }
//     }

//     private bool SomeAdditionalLogic(AuthorizationFilterContext context)
//     {
//         // Özel iş mantığınıza göre burada bir kontrol yapabilirsiniz
//         return false; // Varsayılan olarak her zaman izin ver
//     }
// }