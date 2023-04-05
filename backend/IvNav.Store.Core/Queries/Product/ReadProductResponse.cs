using IvNav.Store.Core.Models.Product;

namespace IvNav.Store.Core.Queries.Product;

public sealed class ReadProductResponse
{
    public ProductModel? Item { get; set; }

    internal ReadProductResponse(ProductModel? item)
    {
        Item = item;
    }
}
