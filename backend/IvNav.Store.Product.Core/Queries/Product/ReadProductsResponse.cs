using Ardalis.GuardClauses;
using IvNav.Store.Product.Core.Models.Product;

namespace IvNav.Store.Product.Core.Queries.Product;

public sealed class ReadProductsResponse
{
    public IReadOnlyCollection<ProductModel> Items { get; set; }

    public int TotalCount { get; set; }

    internal ReadProductsResponse(IEnumerable<ProductModel> items, int totalCount)
    {
        Items = Guard.Against.Null(items).ToList();
        TotalCount = Guard.Against.Negative(totalCount);
    }
}
