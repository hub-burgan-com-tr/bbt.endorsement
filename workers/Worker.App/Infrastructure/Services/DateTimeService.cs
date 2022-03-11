using Worker.App.Application.Common.Interfaces;

namespace Worker.App.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}