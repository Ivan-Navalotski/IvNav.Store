using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IvNav.Store.Infrastructure.EntityConfigurations.User;

internal class UserConfiguration : IEntityTypeConfiguration<Entities.Identity.User>
{
    public void Configure(EntityTypeBuilder<Entities.Identity.User> builder)
    {
        builder
            .HasKey(item => item.Id);
        builder
            .Property(i => i.Id)
            .ValueGeneratedOnAdd();

        builder
            .Property(i => i.DateOfBirth)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasConversion(i =>
                i != null ? i.Value.ToString("O") : null,
                i => i != null ? DateOnly.FromDateTime(DateTime.Parse(i)) : null);
    }
}
