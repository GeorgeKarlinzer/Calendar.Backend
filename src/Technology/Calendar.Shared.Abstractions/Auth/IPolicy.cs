using Microsoft.AspNetCore.Authorization;

namespace Calendar.Shared.Abstractions.Auth;

public interface IPolicy
{
    IEnumerable<IAuthorizationRequirement> Requirements { get; }
}
