using Calendar.Shared.Abstractions.Exceptions;

namespace Calendar.Shared.Infrastructure.Auth.Exceptions
{
    internal class PolicyNotFoundException : CalendarException
    {
        public string PolicyName { get; }

        public PolicyNotFoundException(string policyName) : base($"Authorization policy '{policyName}' was not found!")
        {
            PolicyName = policyName;
        }
    }
}
