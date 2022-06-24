namespace Infrastructure.SsoServices.Models;

public class AccessToken
{
    public string access_token { get; set; }
    public string Tckn { get; set; }
    public List<string> credentials { get; set; }
}