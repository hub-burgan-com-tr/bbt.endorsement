namespace Worker.App.Infrastructure.Logging.Options;

public class EventLogOptions
{
    public bool IsEnabled { get; set; }

    public string LogName { get; set; }

    public string SourceName { get; set; }
}