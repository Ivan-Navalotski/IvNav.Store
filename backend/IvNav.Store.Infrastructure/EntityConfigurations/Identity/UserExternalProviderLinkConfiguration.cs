using IvNav.Store.Infrastructure.Entities.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IvNav.Store.Infrastructure.EntityConfigurations.Identity;

internal class UserExternalProviderLinkConfiguration
{
    public void Configure(EntityTypeBuilder<UserExternalProviderLink> builder)
    {
        builder
            .HasOne(i => i.User)
            .WithMany(i => i.ExternalProviderLinks)
            .HasForeignKey(i => i.UserId);
    }
}
