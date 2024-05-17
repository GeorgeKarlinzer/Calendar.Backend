using CryptLearn.Shared.Abstractions.Auth;

namespace CryptLearn.Modules.AccessControl.Core.Interfaces
{
    internal interface IPermissionClaimsSynchronizer
    {
        Task Synchronize(IEnumerable<IPermissionClaimsProvider> claimsProviders, CancellationToken cancellationToken);
    }
}
