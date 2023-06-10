using IvNav.Store.Product.Core.Models.Product;

namespace IvNav.Store.Product.Core.Extensions.Entities;

internal static class ProductExtensions
{
    internal static ProductModel MapToModel(this Infrastructure.Entities.Product entity)
    {
        return new ProductModel
        {
            Id = entity.Id,
            Name = entity.Name,
        };
    }
}
