using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IvNav.Store.Product.Infrastructure.EntityConfigurations;

internal class ProductConfiguration : IEntityTypeConfiguration<Entities.Product>
{
    public void Configure(EntityTypeBuilder<Entities.Product> builder)
    {
        builder
            .HasKey(i => i.Id);
        builder
            .Property(i => i.Id).ValueGeneratedOnAdd();
    }
}
