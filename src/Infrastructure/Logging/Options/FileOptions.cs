using Serilog.Events;

namespace Infrastructure.Logging.Options;

public class FileOptions
{
    public LogEventLevel MinimumLogEventLevel { get; set; }
}