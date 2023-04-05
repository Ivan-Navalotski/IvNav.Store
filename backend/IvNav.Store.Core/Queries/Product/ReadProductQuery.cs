using IvNav.Store.Core.Extensions.Entities;
using IvNav.Store.Core.Models.Product;
using IvNav.Store.Infrastructure.Abstractions.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IvNav.Store.Core.Queries.Product;

internal sealed class ReadProductQuery : IRequestHandler<ReadProductRequest, ReadProductResponse>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public ReadProductQuery(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<ReadProductResponse> Handle(ReadProductRequest request, CancellationToken cancellationToken)
    {
        var entity = await _applicationDbContext.Products
            .Where(i=> i.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);


        return new ReadProductResponse(entity?.MapToModel());
    }
}
