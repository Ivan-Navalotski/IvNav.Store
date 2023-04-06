using IvNav.Store.Core.Queries.Product;
using IvNav.Store.Core.Tests.Helpers;
using NUnit.Framework;
using IvNav.Store.Core.Tests.Helpers.EntityBuilders;

namespace IvNav.Store.Core.Tests.Queries.Product;

[TestFixture]
internal class ReadProductsQueryTests : TestFixture
{
    private readonly ReadProductsQuery _query;

    public ReadProductsQueryTests()
    {
        _query = new ReadProductsQuery(ApplicationDbContext);
    }

    [Test]
    public async Task When_ReadProductQuery_Expect_Value()
    {
        // arrange
        var products = new List<Infrastructure.Entities.Product>
        {
            await this.CreateProduct(),
            await this.CreateProduct()
        };

        var request = new ReadProductsRequest(null, null);

        // act
        var response = await _query.Handle(request, default);

        // assert
        Assert.IsNotNull(response);
        Assert.IsNotNull(response.Items);

        for (var i = 0; i < products.Count; i++)
        {
            Assert.That(products[i].Id, Is.EqualTo(response.Items.ElementAt(i).Id));
            Assert.That(products[i].Name, Is.EqualTo(response.Items.ElementAt(i).Name));
        }
    }

    [Test]
    public async Task When_ReadProductQuery_Expect_Null()
    {
        // arrange
        var request = new ReadProductsRequest(null, null);

        // act
        var response = await _query.Handle(request, default);

        // assert
        Assert.IsNotNull(response);
        Assert.IsNotNull(response.Items);
        Assert.That(0, Is.EqualTo(response.Items.Count));
    }
}

