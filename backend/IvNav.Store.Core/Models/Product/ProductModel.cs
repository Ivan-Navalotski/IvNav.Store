namespace IvNav.Store.Core.Models.Product;

public sealed class ProductModel
{
    public Guid Id { get; init; }

    public string Name { get; init; } = null!;

    internal static ProductModel MapFromEntity(Infrastructure.Entities.Product product)
    {
        return new ProductModel
        {
            Id = product.Id,
            Name = product.Name,
        };
    }
}
