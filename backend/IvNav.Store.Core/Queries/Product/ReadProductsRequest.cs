using IvNav.Store.Core.Models.Abstractions.Paging;
using MediatR;

namespace IvNav.Store.Core.Queries.Product;

public sealed class ReadProductsRequest : IRequest<ReadProductsResponse>, IPagingRequest
{
    public int? Page { get; }
    public int? PageSize { get; }

    public ReadProductsRequest(int? page, int? pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }
}
