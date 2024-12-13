using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Serilog;
using System.Linq;
[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
public class AuthorizeUserAttribute : AuthorizeAttribute, IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
        Log.Information("OnActionExecuted Headers geldi");
        if (context?.HttpContext?.User?.Identity?.IsAuthenticated == true)
        {
            Log.Information("OnActionExecuted Headers IsAuthenticated");

        }

        Log.Information("OnActionExecuted Headers username" + context?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == "username").Value?.ToString());
        Log.Information("OnActionExecuted Headers FirstOrDefault" + context?.HttpContext?.User?.Claims?.FirstOrDefault().Value?.ToString());
        Log.Information("OnActionExecuted Headers LastOrDefault" + context?.HttpContext?.User?.Claims?.LastOrDefault().Value?.ToString());
    }
    public void OnActionExecuting(ActionExecutingContext context)
    {


        Log.Information("OnActionExecuting Headers:" + JsonConvert.SerializeObject(context.HttpContext?.Request?.Headers));
        // Log.Information("OnActionExecuting User: " + JsonConvert.SerializeObject(context.HttpContext?.User));
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