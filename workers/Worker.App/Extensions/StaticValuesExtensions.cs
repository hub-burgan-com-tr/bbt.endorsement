using Worker.App.Application.Common.Models;
using Worker.App.Infrastructure.Configuration.Options;

namespace Worker.App.Extensions;

public static class StaticValuesExtensions
{
    public static void SetStaticValues(AppSettings settings)
    {
        StaticValues.Sso = settings.Entegration.Sso;
        StaticValues.Internals = settings.Entegration.Internals;
        StaticValues.DMSService = settings.ServiceEndpoint.DMSService;
        StaticValues.TemplateEngine = settings.Entegration.TemplateEngine;
        StaticValues.MessagingGateway = settings.Entegration.MessagingGateway;
        StaticValues.TsizlUrl = settings.Entegration.TsizlUrl;
        StaticValues.ContractUrl = settings.Entegration.ContractUrl;
        StaticValues.AmorphieWorkflowUrl = settings.Entegration.AmorphieWorkflowUrl;
        StaticValues.ContractUserCode = settings.Entegration.ContractUserCode;
    }
}

