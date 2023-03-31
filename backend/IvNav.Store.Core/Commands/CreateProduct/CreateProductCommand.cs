using IvNav.Store.Infrastructure.Abstractions.Contexts;
using IvNav.Store.Infrastructure.Entities;
using MediatR;

namespace IvNav.Store.Core.Commands.CreateProduct;

internal class CreateProductCommand : IRequestHandler<CreateProductRequest, CreateProductResponse>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public CreateProductCommand(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<CreateProductResponse> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var entity = new Product(request.Name);
        _applicationDbContext.Products.Add(entity);

        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return new CreateProductResponse(entity.Id);
    }
}
