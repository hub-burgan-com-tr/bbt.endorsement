namespace Infrastructure.SsoServices.Models;

public class AccessToken
{
    public string citizenshipNumber { get; set; }
    public string customerNumber { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string isStaff { get; set; }
    public string access_token { get; set; }
    public List<string> credentials { get; set; }
}