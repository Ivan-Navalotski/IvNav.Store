using IvNav.Store.Core.Models.Product;

namespace IvNav.Store.Core.Extensions.Entities;

internal static class ProductExtensions
{
    internal static ProductModel MapToModel(this Infrastructure.Entities.Product product)
    {
        return new ProductModel
        {
            Id = product.Id,
            Name = product.Name,
        };
    }
}
