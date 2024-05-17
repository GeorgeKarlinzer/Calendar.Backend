using CryptLearn.Modules.AccessControl.Core.Entities;

namespace CryptLearn.Modules.AccessControl.Core.Interfaces
{
    internal interface IPermissionClaimsRepository
    {
        IQueryable<PermissionClaim> GetAll();
        Task AddAsync(PermissionClaim claim, CancellationToken cancellationToken);
        void Remove(PermissionClaim claim);
        Task SaveAsync(CancellationToken cancellationToken);
    }
}
