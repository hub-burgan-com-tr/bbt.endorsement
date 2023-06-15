using Application.Common.Models;
using Application.SSOIntegrationService.Models;
using Application.TsizlFora.Model;
using Application.TsizlLFora;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ITsizlForaService
    {
        Task<Response<TSIZLResponse>> DoAutomaticEngagementPlain(string accountBranchCode, string accountNumber);
    }
}
