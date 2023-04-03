using IvNav.Store.Infrastructure.Abstractions.Contexts;
using MediatR;

namespace IvNav.Store.Core.Commands.Product;

internal sealed class CreateProductCommand : IRequestHandler<CreateProductRequest, CreateProductResponse>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public CreateProductCommand(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<CreateProductResponse> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var entity = new Infrastructure.Entities.Product(request.Name);
        _applicationDbContext.Products.Add(entity);

        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return new CreateProductResponse(entity.Id);
    }
}
