using AutoMapper;
using IvNav.Store.Identity.Core.Models.User;
using IvNav.Store.Identity.Core.Queries.User;
using IvNav.Store.Identity.Web.Controllers.Api.V1;
using IvNav.Store.Identity.Web.Helpers.Mapper;
using IvNav.Store.Identity.Web.Models.V1.User;
using IvNav.Store.Testing.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace IvNav.Store.Identity.Web.Tests.Controllers.Api.V1;

[TestFixture]
internal class UserControllerTests : IdentityWebTestFixture
{
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _controller = new(MediatorMock.Object, Mapper);
    }

    [Test]
    public async Task When_ReadProduct_Expect_200()
    {
       // arrange
       var userId = UserId;
        var response = new ReadUserResponse(new UserModel
        {
            Id = userId,
            GivenName = "GivenName",
            Surname = "Surname",
            DateOfBirth = new DateOnly(2020, 10, 01),
            NeedSetupPassword = false,
            Phone = "+123456789",
        });

        MediatorMock
            .Setup(mediator => mediator.Send(It.IsAny<ReadUserRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // act
        var httpResponse = await _controller.UserInfo(default);

        // assert
        Assert.That(httpResponse, Is.Not.Null);
        Assert.That(httpResponse, Is.TypeOf<OkObjectResult>());

        var result = ((OkObjectResult)httpResponse).Value;
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.TypeOf<UserInfoResponseDto>());
        MediatorMock.Verify(mediator => mediator.Send(It.IsAny<ReadUserRequest>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
