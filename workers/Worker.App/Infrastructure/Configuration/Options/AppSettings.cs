using Worker.App.Infrastructure.Logging.Options;
using Worker.App.Infrastructure.Persistence.Options;
using Worker.App.Infrastructure.Services.ZeebeServices;

namespace Worker.App.Infrastructure.Configuration.Options;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; }
    public LoggingOptions Logging { get; set; }
    public ZeebeOptions Zeebe { get; set; }
    public Entegration Entegration { get; set; }
}

public class Entegration
{
    public string Internals { get; set; }
    public string TemplateEngine { get; set; }
    public string Sso { get; set; }
    public string DMSService { get; set; }
    public string MessagingGateway { get; set; }
}