using IvNav.Store.Infrastructure.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IvNav.Store.Infrastructure.EntityConfigurations.User;

internal class UserExternalProviderLinkConfiguration : IEntityTypeConfiguration<UserExternalProviderLink>
{
    public void Configure(EntityTypeBuilder<UserExternalProviderLink> builder)
    {
        builder
            .HasOne(i => i.User)
            .WithMany(i => i.ExternalProviderLinks)
            .HasForeignKey(i => i.UserId);
    }
}
