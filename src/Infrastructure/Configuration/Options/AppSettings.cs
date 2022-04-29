using Infrastructure.Logging.Options;
using Infrastructure.Persistence.Options;
using Infrastructure.ZeebeServices;

namespace Infrastructure.Configuration.Options;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; }
    public LoggingOptions Logging { get; set; }
    public ZeebeOptions Zeebe { get; set; }
    public Entegration Entegration { get; set; }
    public TokenOptions Token { get; set; }
}

public class Entegration
{
    public string Internals { get; set; }
    public string TemplateEngine { get; set; }
    public string Sso { get; set; }
}

public class TokenOptions
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecurityKey { get; set; }
}