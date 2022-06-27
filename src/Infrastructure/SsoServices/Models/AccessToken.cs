namespace Infrastructure.SsoServices.Models;

public class AccessToken
{
    public string CitizenshipNumber { get; set; }
    public string CustomerNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string IsStaff { get; set; }
    public string Access_token { get; set; }
    public List<string> Credentials { get; set; }
    public bool IsLogin { get; set; }
}