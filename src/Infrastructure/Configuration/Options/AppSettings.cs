using Infrastructure.Logging.Options;

namespace Infrastructure.Configuration.Options;

public class AppSettings
{
    public string ModelFileName { get; set; }
    public LoggingOptions Logging { get; set; }
}