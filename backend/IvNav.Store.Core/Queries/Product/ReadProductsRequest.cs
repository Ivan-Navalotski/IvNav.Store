using IvNav.Store.Core.Models.Abstractions.Paging;
using MediatR;

namespace IvNav.Store.Core.Queries.Product;

public sealed class ReadProductsRequest : IRequest<ReadProductsResponse>, IOffsetLimitRequest
{
    public int? Offset { get; }
    public int? Limit { get; }

    public ReadProductsRequest(int? offset, int? limit)
    {
        Offset = offset;
        Limit = limit;
    }
}
