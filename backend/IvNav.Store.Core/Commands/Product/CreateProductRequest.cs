using Ardalis.GuardClauses;
using MediatR;

namespace IvNav.Store.Core.Commands.Product;

public class CreateProductRequest : IRequest<CreateProductResponse>
{
    public string Name { get; }

    public CreateProductRequest(string name)
    {
        Name = Guard.Against.NullOrEmpty(name);
    }
}
