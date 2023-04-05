using Ardalis.GuardClauses;
using MediatR;

namespace IvNav.Store.Core.Queries.Product;

public sealed class ReadProductRequest : IRequest<ReadProductResponse>
{
    public Guid Id { get; }

    public ReadProductRequest(Guid id)
    {
        Id = Guard.Against.Default(id);
    }
}
