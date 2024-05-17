using Calendar.Shared.Abstractions.Auth;

namespace Calendar.Identity.Core.Interfaces;

internal interface IPermissionClaimsSynchronizer
{
    Task Synchronize(IEnumerable<IPermissionClaimsProvider> claimsProviders, CancellationToken cancellationToken);
}
