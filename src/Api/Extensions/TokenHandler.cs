using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static Api.Controllers.HomeController;

namespace Api.Extensions;

public class TokenHandler
{
    public IConfiguration Configuration { get; set; }
    public TokenHandler(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public TokenModel CreateAccessToken(UserModel user)
    {
        var tokenInstance = new TokenModel();
        var claims = new Claim[]{
                new Claim(JwtRegisteredClaimNames.NameId,Guid.NewGuid().ToString()),
                new Claim("CitizenshipNumber",user.CitizenshipNumber.ToString()),
                new Claim("ClientNumber",user.ClientNumber.ToString()),
                new Claim("First",user.Name.First.ToString()),
                new Claim("Last",user.Name.Last.ToString()),
                new Claim("IsCustomer",user.IsCustomer.ToString())
            };

        if (user.IsCustomer == false)
        {
            claims = new Claim[]{
                new Claim(JwtRegisteredClaimNames.NameId,Guid.NewGuid().ToString()),
                new Claim("CitizenshipNumber",user.CitizenshipNumber.ToString()),
                new Claim("ClientNumber",user.ClientNumber.ToString()),
                new Claim("First",user.Name.First.ToString()),
                new Claim("Last",user.Name.Last.ToString()),
                new Claim("IsCustomer",user.IsCustomer.ToString()),
                new Claim("IsBranchApproval",user.Authory.IsBranchApproval.ToString()),
                new Claim("IsBranchFormReader",user.Authory.IsBranchFormReader.ToString()),
                new Claim("IsFormReader",user.Authory.IsFormReader.ToString()),
                new Claim("IsNewFormCreator",user.Authory.IsNewFormCreator.ToString()),
                new Claim("IsReadyFormCreator",user.Authory.IsReadyFormCreator.ToString()),
            };
        }

        //Security  Key'in simetriğini alıyoruz.
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:SecurityKey"]));

        //Şifrelenmiş kimliği oluşturuyoruz.
        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //Oluşturulacak token ayarlarını veriyoruz.
        tokenInstance.Expiration = DateTime.Now.AddMinutes(60);
        JwtSecurityToken securityToken = new JwtSecurityToken(
            issuer: Configuration["Token:Issuer"],
            audience: Configuration["Token:Audience"],
            expires: tokenInstance.Expiration,//Token süresini 5 dk olarak belirliyorum
            notBefore: DateTime.Now,//Token üretildikten ne kadar süre sonra devreye girsin ayarlıyouz.
            signingCredentials: signingCredentials,
            claims: claims
            );

        //Token oluşturucu sınıfında bir örnek alıyoruz.
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        //Token üretiyoruz.
        tokenInstance.Token = tokenHandler.WriteToken(securityToken);

        //Refresh Token üretiyoruz.
        tokenInstance.RefreshToken = CreateRefreshToken();
        return tokenInstance;
    }

    public string CreateRefreshToken()
    {
        byte[] number = new byte[32];
        using (RandomNumberGenerator random = RandomNumberGenerator.Create())
        {
            random.GetBytes(number);
            return Convert.ToBase64String(number);
        }
    }
}

public class TokenModel
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public string RefreshToken { get; set; }
}