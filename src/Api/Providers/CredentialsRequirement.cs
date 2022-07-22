using Microsoft.AspNetCore.Authorization;

namespace Api.Providers;

public class CredentialsRequirement : IAuthorizationRequirement
{
    public IDictionary<string, string> AllowedCredentials { get; private set; }

    public CredentialsRequirement(IDictionary<string, string> allowedCredentials)
    {
        AllowedCredentials = allowedCredentials;
    }
}