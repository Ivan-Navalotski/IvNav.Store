using IvNav.Store.Infrastructure.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IvNav.Store.Infrastructure.EntityConfigurations.Identity;

internal class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(item => item.Id);
        builder.HasIndex(i => i.Id).IsUnique();
    }
}
