using Api.Extensions;
using Application.BbtInternals.Queries.GetSearchPersonSummary;
using Infrastructure.SsoServices;
using Infrastructure.SsoServices.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ApiControllerBase
    {
        private readonly IUserService _userService;

        public HomeController( IUserService userService)
        {
            _userService = userService;
        }

        [Route("login")]
        [HttpGet]
        //[SwaggerResponse(200, "Success, queried user search are returned successfully.", typeof(GetSearchPersonSummaryDto))]
        //[SwaggerResponse(404, "Success but there is no user search  available for the query.", typeof(void))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public GetSearchPersonSummaryDto Login()
        {
            try
            {
                string access_token = "";
                string value = Request.Headers["Authorization"];
                if (value.StartsWith("Bearer "))
                {
                    access_token = value.Substring("Bearer ".Length);
                }

                var response = _userService.AccessTokenResource(access_token).Result;

                if (response.IsLogin == false)
                {
                    return new GetSearchPersonSummaryDto { Data = response.IsLogin.ToString() };
                }
                //response = new AccessToken()
                //{
                //    CitizenshipNumber = "12345678901",
                //    CustomerNumber = "0",
                //    IsStaff = "true",
                //};
                //return new GetSearchPersonSummaryDto { Data = "Giriş" };
                var result = new GetSearchPersonSummaryDto
                {
                    CitizenshipNumber = response.CitizenshipNumber,
                    IsStaff = Convert.ToBoolean(response.IsStaff),
                    CustomerNumber = Convert.ToUInt64(response.CustomerNumber),
                    First = response.FirstName,
                    Last = response.LastName,
                    BusinessLine = response.BusinessLine != null ? response.BusinessLine : "",
                    BranchCode = response.BranchCode != null ? response.BranchCode : ""
                };

                GetCredentials(result, response);


                if (result != null)
                {
                    //TokenHandler tokenHandler = new TokenHandler();
                   // Token token = tokenHandler.CreateAccessToken(result);
                    //result.Token = token.AccessToken;
                    Log.Information("Login-Token: " + result.Token);
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }

            return null;
        }

        private void GetCredentials(GetSearchPersonSummaryDto result, AccessToken response)
        {
            if (response.Credentials != null)
            {
                foreach (var credential in response.Credentials)
                {
                    var value = credential.Split("###");
                    if (value.Length == 2)
                    {
                        if (value[0] == "isFormReader")
                        {
                            if (value[1] == "1")
                                result.Authory.IsFormReader = true;
                            else
                                result.Authory.IsFormReader = false;
                        }
                        else if (value[0] == "isNewFormCreator")
                        {
                            if (value[1] == "1")
                                result.Authory.IsNewFormCreator = true;
                            else
                                result.Authory.IsNewFormCreator = false;
                        }
                        else if (value[0] == "isReadyFormCreator")
                        {
                            if (value[1] == "1")
                                result.Authory.IsReadyFormCreator = true;
                            else
                                result.Authory.IsReadyFormCreator = false;
                        }
                        else if (value[0] == "isBranchApproval")
                        {
                            if (value[1] == "1")
                                result.Authory.IsBranchApproval = true;
                            else
                                result.Authory.IsBranchApproval = false;
                        }
                        else if (value[0] == "isBranchFormReader")
                        {
                            if (value[1] == "1")
                                result.Authory.IsBranchFormReader = true;
                            else
                                result.Authory.IsBranchFormReader = false;
                        } 
                        else if (value[0] == "isUIVisible")
                        {
                            if (value[1] == "1")
                                result.Authory.isUIVisible = true;
                            else
                                result.Authory.isUIVisible = false;
                        }
                    }
                }
            }
        }


        //[Route("_login")]
        //[HttpGet]
        //[SwaggerResponse(200, "Success, queried user search are returned successfully.", typeof(UserModel))]
        //[SwaggerResponse(404, "Success but there is no user search  available for the query.", typeof(void))]
        //[ProducesResponseType((int)HttpStatusCode.OK)]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        //public async Task<GetSearchPersonSummaryDto> _Login(string name)
        //{
        //    var isState = false;
        //    if (true)
        //    {
        //        var result = GetUsers()
        //             .Where(x => x.Name.First.Contains(name, StringComparison.OrdinalIgnoreCase) || x.Name.Last.Contains(name, StringComparison.OrdinalIgnoreCase) || (x.Name.First + " " + x.Name.Last).Contains(name, StringComparison.OrdinalIgnoreCase) || x.CitizenshipNumber.ToString() == name || x.CustomerNumber.ToString() == name)
        //            .Select(x => new GetSearchPersonSummaryDto
        //            {
        //                CitizenshipNumber = x.CitizenshipNumber.ToString(),
        //                First = x.Name.First,
        //                Last = x.Name.Last,
        //                CustomerNumber = x.CustomerNumber,
        //                IsStaff = x.IsStaff,
        //            // Email = x.Email,
        //            // TaxNo = x.TaxNo,
        //            // GsmPhone = x.GsmPhone,
        //            Authory = x.IsStaff == true && x.Authory != null ? new AuthoryModel { IsBranchApproval = x.Authory.IsBranchApproval, IsReadyFormCreator = x.Authory.IsReadyFormCreator, IsNewFormCreator = x.Authory.IsNewFormCreator, IsFormReader = x.Authory.IsFormReader, IsBranchFormReader = x.Authory.IsBranchFormReader } : null,
        //            }).FirstOrDefault();

        //        if (result != null)
        //        {
        //            TokenHandler tokenHandler = new TokenHandler(_configuration);
        //            Token token = tokenHandler.CreateAccessToken(result);
        //            result.Token = token.AccessToken;
        //            isState = true;
        //            return result;
        //        }
        //    }


        //    if(isState == false)
        //    {
        //        var response = await Mediator.Send(new GetSearchPersonSummaryQuery() { Name = name });

        //        if (response != null && response.Data.Persons.Any())
        //        {
        //            var result = response.Data.Persons.Select(x => new GetSearchPersonSummaryDto { CustomerNumber = x.CustomerNumber, Token = "", First = x.First, Last = x.Last, CitizenshipNumber = x.CitizenshipNumber, IsStaff = x.IsStaff, Authory = x.Authory }).FirstOrDefault();
        //            TokenHandler tokenHandler = new TokenHandler(_configuration);
        //            Token token = tokenHandler.CreateAccessToken(result);
        //            result.Token = token.AccessToken;
        //            return result;
        //        }
        //    }
        //    return null;
        //}


        //[HttpGet("decode-token")]
        //public JwtSecurityToken DecodeToken(string token)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();

        //    try
        //    {
        //        tokenHandler.ValidateToken(token, new TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(StaticValues.SecretKey)),
        //            ValidateIssuer = false,
        //            ValidateAudience = false,
        //            ValidateLifetime = true,
        //            ClockSkew = TimeSpan.Zero,
        //        }, out SecurityToken validateToken);

        //        var jwtToken = (JwtSecurityToken)validateToken;
        //        return jwtToken;

        //    }
        //    catch 
        //    {
        //    }
        //    var handler = new JwtSecurityTokenHandler();
        //    var jsonToken = handler.ReadJwtToken(token);
        //    var tokenS = jsonToken as JwtSecurityToken;

        //    return tokenS;
        //}

        //[Authorize("roles")]
        //[HttpGet("get")]
        //public int Get()
        //{
        //    var rng = new Random();
        //    var user = User.Claims.FirstOrDefault(c => c.Type == "First").Value;
        //    return rng.Next(-20, 55);

        //}

     

        public class UserModel
        {
        
            public int CustomerNumber { get; set; }
            public long CitizenshipNumber { get; set; }
            public string Token { get; set; }
            public NameClass Name { get; set; }
            public class NameClass
            {
                public string First { get; set; }
                public string Last { get; set; }
            }

            public bool IsStaff { get; set; }
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