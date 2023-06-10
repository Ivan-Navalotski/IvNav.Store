using IvNav.Store.Common.Extensions;
using IvNav.Store.Product.Core.Extensions.Entities;
using IvNav.Store.Product.Core.Models.Product;
using IvNav.Store.Product.Infrastructure.Abstractions.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IvNav.Store.Product.Core.Queries.Product;

internal sealed class ReadProductsQuery : IRequestHandler<ReadProductsRequest, ReadProductsResponse>
{
    private readonly IAppDbContext _appDbContext;

    public ReadProductsQuery(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<ReadProductsResponse> Handle(ReadProductsRequest request, CancellationToken cancellationToken)
    {
        var totalCount = await _appDbContext.Products.CountAsync(cancellationToken);

        if (totalCount == 0)
        {
            return new ReadProductsResponse(Array.Empty<ProductModel>(), totalCount);
        }

        var items = await _appDbContext.Products
            .ApplyPaging(request)
            .Select(i => i.MapToModel())
            .ToListAsync(cancellationToken);

        return new ReadProductsResponse(items, totalCount);
    }
}
