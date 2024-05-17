using Calendar.Identity.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Calendar.Identity.Core.DAL.Configurations;

internal class UserClaimsConfigurations : IEntityTypeConfiguration<UserClaim>
{
    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder.HasKey(x => new { x.UserId, x.Type, x.Value });

        builder.HasOne(x => x.User)
            .WithMany(x => x.Claims)
            .HasForeignKey(x => x.UserId)
            .IsRequired();

        builder.HasOne<PermissionClaim>()
            .WithMany()
            .HasForeignKey(x => new { x.Type, x.Value })
            .IsRequired();
    }
}
