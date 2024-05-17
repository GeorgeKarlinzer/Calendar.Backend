using Calendar.Identity.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Calendar.Identity.Core.DAL.Configurations;

internal class ClaimsConfigurations : IEntityTypeConfiguration<PermissionClaim>
{
    public void Configure(EntityTypeBuilder<PermissionClaim> builder)
    {
        builder.HasKey(x => new { x.Type, x.Value });
    }
}
