using Application.Common.Models;
using Application.SSOIntegrationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ITsizlFora
    {
        Task<Response<string>> DoAutomaticEngagementPlain(string accountBranchCode, string accountNumber);
    }
}
