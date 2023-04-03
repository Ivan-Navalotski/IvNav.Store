using IvNav.Store.Infrastructure.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IvNav.Store.Infrastructure.EntityConfigurations.Identity;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(item => item.Id);
        builder.Property(i => i.Id).ValueGeneratedOnAdd();
    }
}
