using Application.BbtInternals.Queries.GetPersonSummary;
using Application.Common.Interfaces;
using Application.SSOIntegrationService.Commands;
using Domain.Entities;
using Domain.Models;
using Infrastructure.SSOIntegration;
using MediatR;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Api.Extensions;

public static class UserExtensions
{
    public static OrderPerson GetOrderPerson(IEnumerable<Claim> Claims,string UserName)
    {
        var person = new OrderPerson();
        if (Claims.Any(c => c.Type == "credentials"))
        {

            person.CitizenshipNumber = long.Parse(Claims.FirstOrDefault(c => c.Type == "username").Value);
            person.CustomerNumber = int.Parse(Claims.FirstOrDefault(c => c.Type == "customer_number").Value);
            person.First = Claims.FirstOrDefault(c => c.Type == "given_name").Value;
            person.Last = Claims.FirstOrDefault(c => c.Type == "family_name").Value;
            person.BranchCode = Claims.FirstOrDefault(c => c.Type == "branch_id") != null ? Claims.FirstOrDefault(c => c.Type == "branch_id").Value : "";
            person.BusinessLine = Claims.FirstOrDefault(c => c.Type == "business_line") != null ? Claims.FirstOrDefault(c => c.Type == "business_line").Value : "";
            person.Email = Claims.FirstOrDefault(c => c.Type == "email") != null ? Claims.FirstOrDefault(c => c.Type == "email").Value : "";

            var credentials = Claims.Where(c => c.Type == "credentials").ToList();
            foreach (var credential in credentials)
            {
                if (credential.Value == null)
                    continue;

                var value = credential.Value.ToString().Split("###");
                if (value.Length == 2)
                {
                    if (value[0] == "isFormReader")
                    {
                        if (value[1] == "1")
                            person.IsFormReader = true;
                        else
                            person.IsFormReader = false;
                    }
                    else if (value[0] == "isNewFormCreator")
                    {
                        if (value[1] == "1")
                            person.IsNewFormCreator = true;
                        else
                            person.IsNewFormCreator = false;
                    }
                    else if (value[0] == "isReadyFormCreator")
                    {
                        if (value[1] == "1")
                            person.IsReadyFormCreator = true;
                        else
                            person.IsReadyFormCreator = false;
                    }
                    else if (value[0] == "isBranchApproval")
                    {
                        if (value[1] == "1")
                            person.IsBranchApproval = true;
                        else
                            person.IsBranchApproval = false;
                    }
                    else if (value[0] == "isBranchFormReader")
                    {
                        if (value[1] == "1")
                            person.IsBranchFormReader = true;
                        else
                            person.IsBranchFormReader = false;
                    }
                }
            }
        }
        else if(!string.IsNullOrEmpty(UserName))
        {
            var res  =  GetOrderPerson(UserName);
            person = res.Result;
        }

        //IsBranchApproval = Claims.FirstOrDefault(c => c.Type == "IsBranchApproval") != null ? Convert.ToBoolean(Claims.FirstOrDefault(c => c.Type == "IsBranchApproval").Value) : false,
        //    IsBranchFormReader = Claims.FirstOrDefault(c => c.Type == "IsBranchFormReader") != null ? Convert.ToBoolean(Claims.FirstOrDefault(c => c.Type == "IsBranchFormReader").Value) : false,
        //    IsFormReader = Claims.FirstOrDefault(c => c.Type == "IsFormReader") != null ? Convert.ToBoolean(Claims.FirstOrDefault(c => c.Type == "IsFormReader").Value) : false,
        //    IsNewFormCreator = Claims.FirstOrDefault(c => c.Type == "IsNewFormCreator") != null ? Convert.ToBoolean(Claims.FirstOrDefault(c => c.Type == "IsNewFormCreator").Value) : false,
        //    IsReadyFormCreator = Claims.FirstOrDefault(c => c.Type == "IsReadyFormCreator") != null ? Convert.ToBoolean(Claims.FirstOrDefault(c => c.Type == "IsReadyFormCreator").Value) : false,


        return person;
    }
    private static async Task<OrderPerson> GetOrderPerson(string requestUserName)
    {
        var person = new OrderPerson();

        if (string.IsNullOrEmpty(requestUserName))
        {
            return person;
        }
        if (requestUserName.Length < 4)
        {
            return person;
        }
        var res = new SSOIntegrationResponse();
        res.RegisterId = Regex.Match(requestUserName, @"\d+").Value;
        var ssoService = new SSOIntegrationService();

        var resUserByRegisterId = await ssoService.GetUserByRegisterId(res.RegisterId);
        if (resUserByRegisterId.StatusCode == 200)
        {
            res.UserInfo = resUserByRegisterId.Data;
            var resAuthorityForUser = await ssoService.GetAuthorityForUser("MOBIL_ONAY", "Credentials", res.UserInfo.LoginName);
            res.UserAuthorities = resAuthorityForUser.Data;
        }
        SSOResponseMapOrderPerson(res, person);
        return person;
    }
    private static void SSOResponseMapOrderPerson(SSOIntegrationResponse ssoResponse, OrderPerson person)
    {
        person.CitizenshipNumber = Convert.ToInt64(ssoResponse.UserInfo.CitizenshipNumber);
        person.CustomerNumber = Convert.ToInt32(ssoResponse.UserInfo.CustomerNo);
        person.First = ssoResponse.UserInfo.FirstName;
        person.Last = ssoResponse.UserInfo.Surname;
        person.BranchCode = ssoResponse.UserInfo.BranchCode;
        //person.BusinessLine = ssoResponse.UserInfo.BusinessLine;
        person.Email = ssoResponse.UserInfo.Email;
        person.IsBranchFormReader = ssoResponse.UserAuthorities.Any(x => x.Name == "isBranchFormReader" && x.Value == "1");
        person.IsBranchApproval = ssoResponse.UserAuthorities.Any(x => x.Name == "isBranchApproval" && x.Value == "1");
        person.IsReadyFormCreator = ssoResponse.UserAuthorities.Any(x => x.Name == "isReadyFormCreator" && x.Value == "1");
        person.IsNewFormCreator = ssoResponse.UserAuthorities.Any(x => x.Name == "isNewFormCreator" && x.Value == "1");
        person.IsFormReader = ssoResponse.UserAuthorities.Any(x => x.Name == "isFormReader" && x.Value == "1");
    }
    private static OrderPerson GetOrderPerson(IHttpContextAccessor _httpContextAccessor)
    {
        var person = new OrderPerson();
        if (_httpContextAccessor.HttpContext.Session.GetString("CitizenshipNumber") != null)
        {
            person = new OrderPerson
            {
                CitizenshipNumber = long.Parse(_httpContextAccessor.HttpContext.Session.GetString("CitizenshipNumber")),
                CustomerNumber = int.Parse(_httpContextAccessor.HttpContext.Session.GetString("CustomerNumber")),
                BusinessLine = _httpContextAccessor.HttpContext.Session.GetString("BusinessLine"),
                First = _httpContextAccessor.HttpContext.Session.GetString("First"),
                Last = _httpContextAccessor.HttpContext.Session.GetString("Last"),
                BranchCode = _httpContextAccessor.HttpContext.Session.GetString("BranchCode"),
            };

            if (_httpContextAccessor.HttpContext.Session.GetString("IsBranchApproval") != null)
            {
                person.IsBranchApproval = Convert.ToBoolean(_httpContextAccessor.HttpContext.Session.GetString("IsBranchApproval"));
                person.IsBranchFormReader = Convert.ToBoolean(_httpContextAccessor.HttpContext.Session.GetString("IsBranchFormReader"));
                person.IsFormReader = Convert.ToBoolean(_httpContextAccessor.HttpContext.Session.GetString("IsFormReader"));
                person.IsNewFormCreator = Convert.ToBoolean(_httpContextAccessor.HttpContext.Session.GetString("IsNewFormCreator"));
                person.IsReadyFormCreator = Convert.ToBoolean(_httpContextAccessor.HttpContext.Session.GetString("IsReadyFormCreator"));
            }
        }

        return person;
    }
}

