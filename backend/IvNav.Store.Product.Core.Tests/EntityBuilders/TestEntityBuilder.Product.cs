namespace IvNav.Store.Product.Core.Tests.EntityBuilders;

internal static partial class TestEntityBuilder
{
    internal static async Task<Infrastructure.Entities.Product> CreateProduct(this ProductTestFixture test)
    {
        var entity = new Infrastructure.Entities.Product("Test");

        test.AppDbContext.Products.Add(entity);
        await test.AppDbContext.SaveChangesAsync();

        return entity;
    }
}
