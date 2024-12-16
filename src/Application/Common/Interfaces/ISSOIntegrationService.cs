using Application.Common.Models;
using Application.SSOIntegrationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ISSOIntegrationService
    {
        Task<Response<string>> SearchUserInfo(string loginName, string firstName, string lastName);
        Task<Response<List<UserAuthority>>> GetAuthorityForUser(string applicationCode, string authorityName, string loginAndDomainName);
        Task<Response<UserInfo>> GetUserByRegisterId(string registerId);
        Task<Response<string>> GetCustomerByCitizenshipNo(string citizenshipNo);
        Task<Response<string>> GetUserInfoByCustomerNo(string customerNo);
    }
}
