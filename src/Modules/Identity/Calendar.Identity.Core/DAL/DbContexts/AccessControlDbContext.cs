using CryptLearn.Modules.AccessControl.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptLearn.Modules.AccessControl.Core.DAL.DbContexts
{
    internal class AccessControlDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<PermissionClaim> PermissionClaims { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }

        public AccessControlDbContext(DbContextOptions<AccessControlDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
