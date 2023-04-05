using IvNav.Store.Infrastructure.Entities;

namespace IvNav.Store.Core.Tests.Helpers.EntityBuilders;

internal static partial class TestEntityBuilder
{
    internal static async Task<Product> CreateProduct(this DatabaseFixture test)
    {
        var entity = new Product("Test");

        test.ApplicationDbContext.Products.Add(entity);
        await test.ApplicationDbContext.SaveChangesAsync();

        return entity;
    }
}
