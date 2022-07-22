using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Authorization;

namespace Api.Providers;

public static class PolicyProvider
{
    public static void AddPolicies(this AuthorizationOptions options)
    {
        //options.AddPolicy(CanAccessMigrosCashRegisterManager,
        //      policyBuilder => policyBuilder
        //            .AddAuthenticationSchemes(OAuthValidationDefaults.AuthenticationScheme)
        //            .AddRequirements(new CredentialsRequirement(SecurityCredentials.MigrosCashRegisterActionRequirements.ToDictionary())));
    }
}
