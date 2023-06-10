using IvNav.Store.Product.Core.Extensions.Entities;
using IvNav.Store.Product.Infrastructure.Abstractions.Contexts;
using MediatR;

namespace IvNav.Store.Product.Core.Commands.Product;

internal sealed class CreateProductCommand : IRequestHandler<CreateProductRequest, CreateProductResponse>
{
    private readonly IAppDbContext _appDbContext;

    public CreateProductCommand(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<CreateProductResponse> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var entity = new Infrastructure.Entities.Product(request.Name);
        _appDbContext.Products.Add(entity);

        await _appDbContext.SaveChangesAsync(cancellationToken);

        var model = entity.MapToModel();

        return new CreateProductResponse(model);
    }
}
