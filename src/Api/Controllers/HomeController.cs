using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ApiControllerBase
    {    /// <summary>
         /// client_id : tanım bazlı static
         /// grant_type: authorization_code static
         /// client_secret : tanım bazlı static
         /// redirect_uri : tanım bazlı static
         /// ******Giriş yapıldıktan sonra dönen code ve state değeri değeri ile 
         /// 1. olarak entegrasyon kullanıcı ile token üretilir
         /// 2. olarak alınan token bilgisi ile giriş yapan kullanıcının rolleri alınır.
         /// </summary>
         /// <param name="code"></param>
         /// <param name="state"></param>
         /// <returns></returns>
        [Route("login")]
        [HttpGet]
        [SwaggerResponse(200, "Success, queried user search are returned successfully.", typeof(UserModel))]
        [SwaggerResponse(404, "Success but there is no user search  available for the query.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Login(long tckn)
        {
            var response = GetUsers().FirstOrDefault(x => x.Tckn == tckn);
            return Ok(response);
        }

        private List<UserModel> GetUsers()
        {
            var users = new List<UserModel>
            {
                new UserModel { ApproverId = Guid.NewGuid().ToString() }
            };
            return users;
        }


        public class UserModel
        {
            public string ApproverId { get; set; }
            public long Tckn { get; set; }
            public string Name { get; set; }

            public AuthoryModel Authory { get; set; }

            public List<UserModel> Users { get; set; }
        }

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