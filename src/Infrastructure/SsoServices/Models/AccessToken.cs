using System.Text.Json.Serialization;

namespace Infrastructure.SsoServices.Models;

public class AccessToken
{
    public string CitizenshipNumber { get; set; }
    public string CustomerNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string IsStaff { get; set; } // IsStaff==false => ŞUBE, IsStaff==true => Personel
    public string Access_token { get; set; }
    public List<string> Credentials { get; set; }
    public bool IsLogin { get;set; }

    public string BranchCode { get; set; }
    public string BusinessLine { get; set; }
}
public class AuthTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("id_token")]
    public string IdToken { get; set; }

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonPropertyName("refresh_token_expires_in")]
    public int RefreshTokenExpiresIn { get; set; }

    [JsonPropertyName("scope")]
    public string Scope { get; set; }
}

