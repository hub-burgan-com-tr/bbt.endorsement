using Infrastructure.Logging.Options;
using Infrastructure.Persistence.Options;
using Infrastructure.ZeebeServices;

namespace Infrastructure.Configuration.Options;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; }
    public LoggingOptions Logging { get; set; }
    public ZeebeOptions Zeebe { get; set; }
}