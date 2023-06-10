using IvNav.Store.Product.Core.Models.Product;

namespace IvNav.Store.Product.Core.Commands.Product;

public class CreateProductResponse
{
    public ProductModel? Product { get; private set; }

    public CreateProductResponse(ProductModel? product)
    {
        Product = product;
    }
}
