using Infrastructure.Logging.Options;
using Infrastructure.ZeebeServices;

namespace Infrastructure.Configuration.Options;

public class AppSettings
{
    public LoggingOptions Logging { get; set; }
    public ZeebeOptions Zeebe { get; set; }
}