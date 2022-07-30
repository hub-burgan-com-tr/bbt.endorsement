using System.Security.Claims;

namespace Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static long GetCitizenshipNumber(this ClaimsPrincipal principal)
    {
        var citizenshipNumber = long.Parse(principal.Claims.FirstOrDefault(c => c.Type == "username").Value.ToString());
        return citizenshipNumber;
    }
    public static bool IsCredentials(this ClaimsPrincipal principal)
    {
        var credentials = principal.Claims.Where(c => c.Type == "credentials").ToList();
        return credentials.Count > 0;
    }

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
}