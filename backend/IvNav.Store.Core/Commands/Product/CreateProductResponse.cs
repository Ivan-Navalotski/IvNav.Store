namespace IvNav.Store.Core.Commands.Product;

public class CreateProductResponse
{
    public Guid? ProductId { get; private set; }

    public CreateProductResponse(Guid? productId)
    {
        ProductId = productId;
    }
}
