using AutoMapper;
using IvNav.Store.Common.Identity;
using MediatR;
using Moq;
using IvNav.Store.Web.Helpers.Mapper;
using NUnit.Framework;

namespace IvNav.Store.Web.Tests.Helpers;

internal class TestFixture
{
    public Guid UserId { get; }

    internal readonly Mock<IMediator> MediatorMock;
    internal readonly IMapper Mapper;

    public TestFixture()
    {
        MediatorMock = new(MockBehavior.Strict);

        UserId = Guid.NewGuid();
        IdentityState.SetCurrent(UserId, null);

        Mapper = new MapperConfiguration(cfg => cfg.AddProfile(new WebAutoMapperProfile())).CreateMapper();
    }

    [SetUp]
    public virtual void BeforeEachTest()
    {
        MediatorMock.Invocations.Clear();
    }
}
