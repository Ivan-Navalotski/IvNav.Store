using IvNav.Store.Core.Extensions.Common;
using IvNav.Store.Core.Models.Product;
using IvNav.Store.Infrastructure.Abstractions.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IvNav.Store.Core.Queries.Product;

internal sealed class ReadProductsQuery : IRequestHandler<ReadProductsRequest, ReadProductsResponse>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public ReadProductsQuery(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<ReadProductsResponse> Handle(ReadProductsRequest request, CancellationToken cancellationToken)
    {
        var totalCount = await _applicationDbContext.Products.CountAsync(cancellationToken);

        if (totalCount == 0)
        {
            return new ReadProductsResponse(Array.Empty<ProductModel>(), totalCount);
        }

        var items = await _applicationDbContext.Products
            .ApplyPaging(request)
            .Select(i => ProductModel.MapFromEntity(i))
            .ToListAsync(cancellationToken);

        return new ReadProductsResponse(items, totalCount);
    }
}
