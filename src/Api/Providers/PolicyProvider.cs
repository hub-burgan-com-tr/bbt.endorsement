using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Api.Providers;

public static class PolicyProvider
{
    public const string DefaultAuthorizedPolicyManager = "DefaultAuthorizedPolicyManager";

    public static void AddPolicies(this AuthorizationOptions options)
    {
        options.AddPolicy(DefaultAuthorizedPolicyManager,
              policyBuilder => policyBuilder
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .AddRequirements(new CredentialsRequirement(SecurityCredentials.DefaultRegisterActionRequirements.ToDictionary())));
    }
}
