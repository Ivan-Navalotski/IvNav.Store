using IvNav.Store.Core.Models.Product;

namespace IvNav.Store.Core.Commands.Product;

public class CreateProductResponse
{
    public ProductModel? Product { get; private set; }

    public CreateProductResponse(ProductModel? product)
    {
        Product = product;
    }
}
