using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Integration.Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static string GetDMSServiceUrl(this IConfiguration config)
    {
        return config.GetSection("ServiceEndpoint")["DMSService"];
    }
}
