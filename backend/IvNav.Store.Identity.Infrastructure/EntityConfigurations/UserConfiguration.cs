using IvNav.Store.Identity.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IvNav.Store.Identity.Infrastructure.EntityConfigurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasKey(item => item.Id);
        builder
            .Property(i => i.Id)
            .ValueGeneratedOnAdd();
    }
}
