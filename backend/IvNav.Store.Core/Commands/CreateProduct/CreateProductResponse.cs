namespace IvNav.Store.Core.Commands.CreateProduct;

public class CreateProductResponse
{
    public Guid? ProductId { get; private set; }

    public CreateProductResponse(Guid? productId)
    {
        ProductId = productId;
    }
}
