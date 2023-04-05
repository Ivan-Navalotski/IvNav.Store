using IvNav.Store.Core.Tests.Helpers;
using IvNav.Store.Core.Queries.Product;
using IvNav.Store.Core.Tests.Helpers.EntityBuilders;
using NUnit.Framework;

namespace IvNav.Store.Core.Tests.Queries.Product;

[TestFixture]
internal class ReadProductQueryTests : DatabaseFixture
{
    private readonly ReadProductQuery _query;

    public ReadProductQueryTests()
    {
        _query = new ReadProductQuery(ApplicationDbContext);
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
        Assert.IsNotNull(response);
        Assert.IsNotNull(response.Item);

        Assert.AreEqual(product.Id, response.Item!.Id);
        Assert.AreEqual(product.Name, response.Item.Name);
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
        Assert.IsNotNull(response);
        Assert.IsNull(response.Item);
    }
}
