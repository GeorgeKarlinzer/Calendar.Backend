using Calendar.Shared.Abstractions.Exceptions;

namespace Calendar.Identity.Core.Exceptions;

internal class CurrentPasswordIsIncorrectException : CalendarException
{
    public CurrentPasswordIsIncorrectException() : base("Current password is incorrect")
    {
    }
}
