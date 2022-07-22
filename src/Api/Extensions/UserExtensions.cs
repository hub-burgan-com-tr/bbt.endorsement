using Domain.Models;
using System.Security.Claims;

namespace Api.Extensions;

public static class UserExtensions
{
    public static OrderPerson GetOrderPerson(IEnumerable<Claim> Claims)
    {
        var person = new OrderPerson
        {
            CitizenshipNumber = long.Parse(Claims.FirstOrDefault(c => c.Type == "CitizenshipNumber").Value),
            CustomerNumber = int.Parse(Claims.FirstOrDefault(c => c.Type == "CustomerNumber").Value),
            First = Claims.FirstOrDefault(c => c.Type == "First").Value,
            Last = Claims.FirstOrDefault(c => c.Type == "Last").Value,
            BranchCode = Claims.FirstOrDefault(c => c.Type == "BranchCode") != null ? Claims.FirstOrDefault(c => c.Type == "BranchCode").Value : "",
            BusinessLine = Claims.FirstOrDefault(c => c.Type == "BusinessLine") != null ? Claims.FirstOrDefault(c => c.Type == "BusinessLine").Value : "",

            IsBranchApproval = Claims.FirstOrDefault(c => c.Type == "IsBranchApproval") != null ? Convert.ToBoolean(Claims.FirstOrDefault(c => c.Type == "IsBranchApproval").Value) : false,
            IsBranchFormReader = Claims.FirstOrDefault(c => c.Type == "IsBranchFormReader") != null ? Convert.ToBoolean(Claims.FirstOrDefault(c => c.Type == "IsBranchFormReader").Value) : false,
            IsFormReader = Claims.FirstOrDefault(c => c.Type == "IsFormReader") != null ? Convert.ToBoolean(Claims.FirstOrDefault(c => c.Type == "IsFormReader").Value) : false,
            IsNewFormCreator = Claims.FirstOrDefault(c => c.Type == "IsNewFormCreator") != null ? Convert.ToBoolean(Claims.FirstOrDefault(c => c.Type == "IsNewFormCreator").Value) : false,
            IsReadyFormCreator = Claims.FirstOrDefault(c => c.Type == "IsReadyFormCreator") != null ? Convert.ToBoolean(Claims.FirstOrDefault(c => c.Type == "IsReadyFormCreator").Value) : false,
        };

        return person;
    }

    public static OrderPerson GetOrderPerson(IHttpContextAccessor _httpContextAccessor)
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

