using Api.Extensions;
using Application.BbtInternals.Queries.GetSearchPersonSummary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ApiControllerBase
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [Route("login")]
        [HttpGet]
        [SwaggerResponse(200, "Success, queried user search are returned successfully.", typeof(UserModel))]
        [SwaggerResponse(404, "Success but there is no user search  available for the query.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<GetSearchPersonSummaryDto> Login(string name)
        {

            var response = await Mediator.Send(new GetSearchPersonSummaryQuery() { Name = name });
      
            if (response != null&&response.Data.Persons.Any())
            {
                var result = response.Data.Persons.Select(x => new GetSearchPersonSummaryDto {ClientNumber=x.ClientNumber,Token="", First = x.First, Last = x.Last, CitizenshipNumber = x.CitizenshipNumber, IsCustomer = x.IsCustomer, Authory = x.Authory }).FirstOrDefault();
                TokenHandler tokenHandler = new TokenHandler(_configuration);
                Token token = tokenHandler.CreateAccessToken(result);
                result.Token = token.AccessToken;
                return result;
                
            }
            return null;
        }


        //[HttpGet("decode-token")]
        //public JwtSecurityToken DecodeToken(string token)
        //{
        //    var handler = new JwtSecurityTokenHandler();
        //    var jsonToken = handler.ReadToken(token);
        //    var tokenS = jsonToken as JwtSecurityToken;

        //    return tokenS;
        //}

        //[Authorize]
        //[HttpGet("get")]
        //public int Get()
        //{
        //    var rng = new Random();
        //    var user = User.Claims.FirstOrDefault(c => c.Type == "First").Value;
        //    return rng.Next(-20, 55);

        //}

        private List<UserModel> GetUsers()
        {
            var users = new List<UserModel>
            {
                new UserModel { CitizenshipNumber = 56906430700, ClientNumber = 1309912, Name = new UserModel.NameClass { First = "Ahmet", Last = "Güler" }, IsCustomer = true },
                new UserModel { CitizenshipNumber = 77120263424, ClientNumber = 3063809, Name = new UserModel.NameClass { First = "Hüseyin", Last = "Töremen" }, IsCustomer = true },
                new UserModel { CitizenshipNumber = 69967514210, ClientNumber = 3580693, Name = new UserModel.NameClass { First = "Gizem", Last = "Ünal" }, IsCustomer = true },
                new UserModel { CitizenshipNumber = 58542320728, ClientNumber = 1324223, Name = new UserModel.NameClass { First = "Tolgahan", Last = "Özgür" }, IsCustomer = true },
                new UserModel { CitizenshipNumber = 17556080776, ClientNumber = 5142508, Name = new UserModel.NameClass { First = "Merve", Last = "Aydın" }, IsCustomer = true },
                new UserModel { CitizenshipNumber = 70189942774, ClientNumber = 4362433, Name = new UserModel.NameClass { First = "Mehmet Ali", Last = "Çokyaşar" }, IsCustomer = true },
                
                // Personel Yetki
                new UserModel { CitizenshipNumber = 21216850128, ClientNumber = 4830830, Name = new UserModel.NameClass { First = "Yetki", Last = "Yok" }, IsCustomer = false, Authory = new UserModel.AuthoryModel{ IsBranchApproval = false, IsBranchFormReader = false, IsFormReader = false, IsNewFormCreator = false, IsReadyFormCreator = false, }},
                
                // Personel                
                new UserModel { CitizenshipNumber = 55871259316, ClientNumber = 1340653, Name = new UserModel.NameClass { First = "Yetki", Last = "1" }, IsCustomer = false, Authory = new UserModel.AuthoryModel{ IsBranchApproval = false, IsBranchFormReader = false, IsFormReader = true, IsNewFormCreator = false, IsReadyFormCreator = false }},
                new UserModel { CitizenshipNumber = 59976413048, ClientNumber = 4788897, Name = new UserModel.NameClass { First = "Yetki", Last = "2" }, IsCustomer = false, Authory = new UserModel.AuthoryModel{ IsBranchApproval = false, IsBranchFormReader = false, IsFormReader = true, IsNewFormCreator = true, IsReadyFormCreator = false }},
                new UserModel { CitizenshipNumber = 31971649998, ClientNumber = 2977276, Name = new UserModel.NameClass { First = "Yetki", Last = "3" }, IsCustomer = false, Authory = new UserModel.AuthoryModel{ IsBranchApproval = false, IsBranchFormReader = false, IsFormReader = true, IsNewFormCreator = false, IsReadyFormCreator = true }},
                new UserModel { CitizenshipNumber = 48324769968, ClientNumber = 4135519, Name = new UserModel.NameClass { First = "Yetki", Last = "4" }, IsCustomer = false, Authory = new UserModel.AuthoryModel{ IsBranchApproval = false, IsBranchFormReader = false, IsFormReader = true, IsNewFormCreator = true, IsReadyFormCreator = true }},
         
                // Şube
                new UserModel { CitizenshipNumber = 26556716738, ClientNumber = 9920213, Name = new UserModel.NameClass { First = "Yetki Şube", Last = "1" }, IsCustomer = false, Authory = new UserModel.AuthoryModel{ IsBranchApproval = false, IsBranchFormReader = false, IsFormReader = true, IsNewFormCreator = false, IsReadyFormCreator = false }},
                new UserModel { CitizenshipNumber = 65628864892, ClientNumber = 6855588, Name = new UserModel.NameClass { First = "Yetki Şube", Last = "2" }, IsCustomer = false, Authory = new UserModel.AuthoryModel{ IsBranchApproval = true, IsBranchFormReader = false, IsFormReader = true, IsNewFormCreator = false, IsReadyFormCreator = false }},
                new UserModel { CitizenshipNumber = 20778668004, ClientNumber = 5266452, Name = new UserModel.NameClass { First = "Yetki Şube", Last = "3" }, IsCustomer = false, Authory = new UserModel.AuthoryModel{ IsBranchApproval = false, IsBranchFormReader = true, IsFormReader = true, IsNewFormCreator = false, IsReadyFormCreator = false }},
                new UserModel { CitizenshipNumber = 95445410934, ClientNumber = 4435967, Name = new UserModel.NameClass { First = "Yetki Şube", Last = "4" }, IsCustomer = false, Authory = new UserModel.AuthoryModel{ IsBranchApproval = true, IsBranchFormReader = true, IsFormReader = true, IsNewFormCreator = false, IsReadyFormCreator = false }},
            };
            return users;
        }


        public class UserModel
        {
        
            public long ClientNumber { get; set; }
            public long CitizenshipNumber { get; set; }
            public string Token { get; set; }
            public NameClass Name { get; set; }
            public class NameClass
            {
                public string First { get; set; }
                public string Last { get; set; }
            }

            public bool IsCustomer { get; set; }
            public AuthoryModel Authory { get; set; }

            public class AuthoryModel
            {
                public bool IsReadyFormCreator { get; set; } // Form ile Emir Oluşturma
                public bool IsNewFormCreator { get; set; } //Yeni Onay Emri Oluşturma
                public bool IsFormReader { get; set; } // Tüm Onay Emirlerini İzleyebilir
                public bool IsBranchFormReader { get; set; } //Farklı Şube Onay İsteme
                public bool IsBranchApproval { get; set; } //Farklı Şube Onay Listeleme
            }
        }
    }
}