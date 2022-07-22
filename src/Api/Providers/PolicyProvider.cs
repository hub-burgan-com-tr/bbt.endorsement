using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Authorization;

namespace Api.Providers;

public static class PolicyProvider
{
    public const string DefaultAuthorizedPolicyManager = "DefaultAuthorizedPolicyManager";

    public static void AddPolicies(this AuthorizationOptions options)
    {
        options.AddPolicy(DefaultAuthorizedPolicyManager,
              policyBuilder => policyBuilder
                    .AddAuthenticationSchemes(OAuthValidationDefaults.AuthenticationScheme)
                    .AddRequirements(new CredentialsRequirement(SecurityCredentials.DefaultRegisterActionRequirements.ToDictionary())));
    }
}
