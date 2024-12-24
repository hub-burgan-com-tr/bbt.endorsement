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
    public string AccessToken { get; set; }
    public string IdToken { get; set; }
    public string RefreshToken { get; set; }
    public string TokenType { get; set; }
    public int ExpiresIn { get; set; }
    public int RefreshTokenExpiresIn { get; set; }
    public string Scope { get; set; }
}
