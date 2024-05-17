using CryptLearn.Modules.AccessControl.Core.Entities;

namespace CryptLearn.Modules.AccessControl.Core.Interfaces
{
    internal interface IUsersRepository
    {
        IQueryable<User> GetAll();
        Task AddAsync(User user, CancellationToken cancellationToken = default);
        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}
