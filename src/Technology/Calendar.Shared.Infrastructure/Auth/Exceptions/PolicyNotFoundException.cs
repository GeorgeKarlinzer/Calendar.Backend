using Calendar.Shared.Abstractions.Exceptions;

namespace Calendar.Shared.Infrastructure.Auth.Exceptions;

internal class PolicyNotFoundException(string policyName) : CalendarException($"Authorization policy '{policyName}' was not found!")
{
    public string PolicyName { get; } = policyName;
}
