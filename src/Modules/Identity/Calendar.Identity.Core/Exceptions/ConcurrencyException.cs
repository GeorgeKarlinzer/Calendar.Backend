using Calendar.Shared.Abstractions.Exceptions;

namespace Calendar.Identity.Core.Exceptions;

internal class ConcurrencyException : CalendarException
{
    public ConcurrencyException() : base("Concurrency error occured!")
    {
    }
}
