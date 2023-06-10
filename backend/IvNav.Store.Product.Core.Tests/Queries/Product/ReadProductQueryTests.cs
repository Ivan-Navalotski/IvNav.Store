using IvNav.Store.Product.Core.Queries.Product;
using IvNav.Store.Product.Core.Tests.EntityBuilders;
using NUnit.Framework;

namespace IvNav.Store.Product.Core.Tests.Queries.Product;

[TestFixture]
internal class ReadProductQueryTests : ProductTestFixture
{
    private readonly ReadProductQuery _query;

    public ReadProductQueryTests()
    {
        _query = new ReadProductQuery(AppDbContext);
    }

    [Test]
    public async Task When_ReadProductQuery_Expect_Value()
    {
        // arrange
        var product = await this.CreateProduct();
        var request = new ReadProductRequest(product.Id);

        // act
        var response = await _query.Handle(request, default);

        // assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Item, Is.Not.Null);

        Assert.That(product.Id, Is.EqualTo(response.Item!.Id));
        Assert.That(product.Name, Is.EqualTo(response.Item.Name));
    }

    [Test]
    public async Task When_ReadProductQuery_Expect_Null()
    {
        // arrange
        var productId = Guid.NewGuid();
        var request = new ReadProductRequest(productId);

        // act
        var response = await _query.Handle(request, default);

        // assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response, Is.EqualTo(ReadProductResponse.NotExists));
    }
}
