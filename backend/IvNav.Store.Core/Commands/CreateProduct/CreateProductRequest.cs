using MediatR;
using Ardalis.GuardClauses;

namespace IvNav.Store.Core.Commands.CreateProduct;

public class CreateProductRequest : IRequest<CreateProductResponse>
{
    public string Name { get; }

    public CreateProductRequest(string name)
    {
        Name = Guard.Against.NullOrEmpty(name);
    }
}
