using Microsoft.EntityFrameworkCore;
using IvNav.Store.Infrastructure.Entities.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IvNav.Store.Infrastructure.EntityConfigurations.Identity;

internal class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(item => new { item.UserId, item.RoleId });
    }
}
