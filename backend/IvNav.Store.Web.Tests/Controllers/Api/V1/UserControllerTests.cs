using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using IvNav.Store.Core.Models.User;
using IvNav.Store.Core.Queries.User;
using IvNav.Store.Web.Controllers.Api.V1;
using IvNav.Store.Web.Models.V1.User;
using IvNav.Store.Web.Tests.Helpers;

namespace IvNav.Store.Web.Tests.Controllers.Api.V1;

[TestFixture]
internal class UserControllerTests : TestFixture
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
