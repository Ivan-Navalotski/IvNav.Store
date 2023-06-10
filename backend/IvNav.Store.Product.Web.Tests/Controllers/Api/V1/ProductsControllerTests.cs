using IvNav.Store.Product.Core.Models.Product;
using IvNav.Store.Product.Core.Queries.Product;
using IvNav.Store.Product.Web.Controllers.Api.V1;
using IvNav.Store.Product.Web.Models.V1.Product;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace IvNav.Store.Product.Web.Tests.Controllers.Api.V1;

[TestFixture]
internal class ProductsControllerTests : ProductWebTestFixture
{
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _controller = new(MediatorMock.Object, Mapper);
    }

    [Test]
    public async Task When_ReadProduct_Expect_200()
    {
        // arrange
        var productId = Guid.NewGuid();
        var response = new ReadProductResponse(new ProductModel
        {
            Id = productId,
            Name = "Test",
        });

        MediatorMock
            .Setup(mediator => mediator.Send(It.IsAny<ReadProductRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // act
        var httpResponse = await _controller.ReadProduct(productId, default);

        // assert
        Assert.That(httpResponse, Is.Not.Null);
        Assert.That(httpResponse, Is.TypeOf<OkObjectResult>());

        var result = ((OkObjectResult)httpResponse).Value;
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<ReadProductResponseDto>());
        MediatorMock.Verify(mediator => mediator.Send(It.IsAny<ReadProductRequest>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task When_ReadProduct_Expect_404()
    {
        // arrange
        var productId = Guid.NewGuid();
        var response = ReadProductResponse.NotExists;

        MediatorMock
            .Setup(mediator => mediator.Send(It.IsAny<ReadProductRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // act
        var httpResponse = await _controller.ReadProduct(productId, default);

        // assert
        Assert.That(httpResponse, Is.Not.Null);
        Assert.That(httpResponse, Is.TypeOf<NotFoundResult>());
        MediatorMock
            .Verify(mediator => mediator.Send(It.IsAny<ReadProductRequest>(), It.IsAny<CancellationToken>()),
                Times.Once);
    }
}
