using Calendar.Shared.Abstractions.Time;

namespace Calendar.Shared.Infrastructure.Time;

internal class UtcClock : IClock
{
    public DateTime CurrentDate() => DateTime.UtcNow;
}