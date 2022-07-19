using Application.Common.Models;
using Infrastructure.Configuration.Options;

namespace Api.Extensions;

public static class StaticValuesExtensions
{
    public static void SetStaticValues(AppSettings settings)
    {
        StaticValues.Sso = settings.Entegration.Sso;
        StaticValues.Internals = settings.Entegration.Internals;
        StaticValues.DMSService = settings.Entegration.DMSService;
        StaticValues.TemplateEngine = settings.Entegration.TemplateEngine;
        StaticValues.ClientSecret = settings.Entegration.ClientSecret;

        StaticValues.Authority = settings.Authentication.Authority;
        StaticValues.ApiGateway = settings.Authentication.ApiGateway;   
        StaticValues.RedirectUri = settings.Authentication.RedirectUri;

        StaticValues.Issuer = settings.Token.Issuer;
        StaticValues.Audience = settings.Token.Audience;
        StaticValues.SecurityKey = settings.Token.SecurityKey;
    }
}

