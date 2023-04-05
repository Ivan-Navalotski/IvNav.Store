using Ardalis.GuardClauses;
using IvNav.Store.Core.Models.Abstractions.Paging;
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
