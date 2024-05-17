using Microsoft.EntityFrameworkCore;

namespace Calendar.Shared.Abstractions.Services;

public interface IRepository
{
    protected DbContext Context { get; }
}
public interface IRepository<T> : IRepository where T : class
{
    IQueryable<T> GetAll()
        => Context.Set<T>();
    async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        => await Context.AddAsync(entity, cancellationToken);
    Task RemoveAsync(T entity, CancellationToken cancellationToken = default)
    {
        Context.Remove(entity);
        return Task.CompletedTask;
    }
    Task SaveAsync(CancellationToken cancellationToken = default)
        => Context.SaveChangesAsync(cancellationToken);
}
