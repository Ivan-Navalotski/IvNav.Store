using AutoMapper;
using MediatR;
using Moq;
using IvNav.Store.Web.Helpers;

namespace IvNav.Store.Web.Tests.Helpers;

internal class MediatorFixture
{
    internal readonly Mock<IMediator> MediatorMock;
    internal readonly IMapper Mapper;

    public MediatorFixture()
    {
        MediatorMock = new(MockBehavior.Strict);

        Mapper = new MapperConfiguration(cfg => cfg.AddProfile(new WebAutoMapperProfile())).CreateMapper();
    }
}
