using AutoMapper;
using IvNav.Store.Core.Models.Product;
using IvNav.Store.Core.Queries.Product;
using IvNav.Store.Web.Controllers.Api.V1;
using IvNav.Store.Web.Helpers;
using IvNav.Store.Web.Models.V1.Product;
using IvNav.Store.Web.Tests.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace IvNav.Store.Web.Tests.Controllers.Api.V1;


internal class ProductsControllerTests : MediatorFixture
{
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _controller = new(MediatorMock.Object, Mapper);

    }

    [Test]
    public async Task When_GetProduct_Expect_200Response()
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
        var httpResponse = await _controller.GetProduct(productId, default);

        // assert
        Assert.NotNull(httpResponse);
        Assert.IsInstanceOf<OkObjectResult>(httpResponse);
        var result = ((OkObjectResult)httpResponse).Value;
        Assert.NotNull(result);
        Assert.IsInstanceOf<ReadProductResponseDto>(result);
    }
}
