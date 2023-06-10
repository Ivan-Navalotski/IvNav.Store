using IvNav.Store.Product.Core.Extensions.Entities;
using IvNav.Store.Product.Infrastructure.Abstractions.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IvNav.Store.Product.Core.Queries.Product;

internal sealed class ReadProductQuery : IRequestHandler<ReadProductRequest, ReadProductResponse>
{
    private readonly IAppDbContext _appDbContext;

    public ReadProductQuery(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<ReadProductResponse> Handle(ReadProductRequest request, CancellationToken cancellationToken)
    {
        var entity = await _appDbContext.Products
            .Where(i => i.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return entity != null
            ? new ReadProductResponse(entity.MapToModel())
            : ReadProductResponse.NotExists;
    }
}
