using CryptLearn.Modules.AccessControl.Core.DAL.DbContexts;
using CryptLearn.Modules.AccessControl.Core.Entities;
using CryptLearn.Modules.AccessControl.Core.Interfaces;

namespace CryptLearn.Modules.AccessControl.Core.DAL.Repositories
{
    internal class PermissionClaimsRepository : IPermissionClaimsRepository
    {
        private readonly AccessControlDbContext _dbContext;

        public PermissionClaimsRepository(AccessControlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(PermissionClaim claim, CancellationToken cancellationToken)
        {
            await _dbContext.AddAsync(claim, cancellationToken);
        }

        public IQueryable<PermissionClaim> GetAll()
        {
            return _dbContext.PermissionClaims;
        }

        public void Remove(PermissionClaim claim)
        {
            _dbContext.Remove(claim);
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
