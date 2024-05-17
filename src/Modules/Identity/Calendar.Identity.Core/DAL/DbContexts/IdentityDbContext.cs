using Calendar.Identity.Core.Entities;
using Calendar.Shared.Abstractions.Services;
using Microsoft.EntityFrameworkCore;

namespace Calendar.Identity.Core.DAL.DbContexts;

internal class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : DbContext(options), IRepository<User>, IRepository<PermissionClaim>
{
    public DbSet<User> Users { get; set; }
    public DbSet<PermissionClaim> PermissionClaims { get; set; }
    public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
    public DbSet<UserClaim> UserClaims { get; set; }

    DbContext IRepository.Context => this;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
