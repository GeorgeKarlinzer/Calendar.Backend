using CryptLearn.Modules.AccessControl.Core.DAL.DbContexts;
using CryptLearn.Modules.AccessControl.Core.Entities;
using CryptLearn.Modules.AccessControl.Core.Interfaces;

namespace CryptLearn.Modules.AccessControl.Core.DAL.Repositories
{
    internal class UsersRepository : IUsersRepository
    {
        private readonly AccessControlDbContext _dbContext;

        public UsersRepository(AccessControlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(user, cancellationToken);
        }

        public IQueryable<User> GetAll()
        {
            return _dbContext.Users;
        }
        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
