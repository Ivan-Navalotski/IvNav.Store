using IvNav.Store.Core.Models.Product;

namespace IvNav.Store.Core.Extensions.Entities;

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
