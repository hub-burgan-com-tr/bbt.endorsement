using Serilog.Events;

namespace Worker.App.Infrastructure.Logging.Options;

public class FileOptions
{
    public LogEventLevel MinimumLogEventLevel { get; set; }
}