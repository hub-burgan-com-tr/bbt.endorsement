using Domain.Models;
using System.Security.Claims;

namespace Api.Extensions;

public static class UserExtensions
{
    public static OrderPerson GetOrderPerson(IEnumerable<Claim> Claims)
    {
        var person = new OrderPerson();

        person.CitizenshipNumber = long.Parse(Claims.FirstOrDefault(c => c.Type == "username").Value);
        person.CustomerNumber = int.Parse(Claims.FirstOrDefault(c => c.Type == "customer_number").Value);
        person.First = Claims.FirstOrDefault(c => c.Type == "given_name").Value;
        person.Last = Claims.FirstOrDefault(c => c.Type == "family_name").Value;
        person.BranchCode = Claims.FirstOrDefault(c => c.Type == "branch_id") != null ? Claims.FirstOrDefault(c => c.Type == "branch_id").Value : "";
        person.BusinessLine = Claims.FirstOrDefault(c => c.Type == "business_line") != null ? Claims.FirstOrDefault(c => c.Type == "business_line").Value : "";

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


        //IsBranchApproval = Claims.FirstOrDefault(c => c.Type == "IsBranchApproval") != null ? Convert.ToBoolean(Claims.FirstOrDefault(c => c.Type == "IsBranchApproval").Value) : false,
        //    IsBranchFormReader = Claims.FirstOrDefault(c => c.Type == "IsBranchFormReader") != null ? Convert.ToBoolean(Claims.FirstOrDefault(c => c.Type == "IsBranchFormReader").Value) : false,
        //    IsFormReader = Claims.FirstOrDefault(c => c.Type == "IsFormReader") != null ? Convert.ToBoolean(Claims.FirstOrDefault(c => c.Type == "IsFormReader").Value) : false,
        //    IsNewFormCreator = Claims.FirstOrDefault(c => c.Type == "IsNewFormCreator") != null ? Convert.ToBoolean(Claims.FirstOrDefault(c => c.Type == "IsNewFormCreator").Value) : false,
        //    IsReadyFormCreator = Claims.FirstOrDefault(c => c.Type == "IsReadyFormCreator") != null ? Convert.ToBoolean(Claims.FirstOrDefault(c => c.Type == "IsReadyFormCreator").Value) : false,

        return person;
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
                BranchCode = _httpContextAccessor.HttpContext.Session.GetString("BranchCode"),};

            if(_httpContextAccessor.HttpContext.Session.GetString("IsBranchApproval") != null)
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

