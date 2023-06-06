using Ardalis.GuardClauses;
using IvNav.Store.Core.Models.Product;

namespace IvNav.Store.Core.Queries.Product;

public sealed class ReadProductResponse
{
    public static ReadProductResponse NotExists = new();

    public ProductModel? Item { get; set; }

    internal ReadProductResponse(ProductModel? item)
    {
        Item = Guard.Against.Null(item);
    }

    private ReadProductResponse()
    {
    }
}
