using AutoMapper;
using IvNav.Store.Common.Identity;
using MediatR;
using Moq;
using NUnit.Framework;

namespace IvNav.Store.Testing.Helpers;

public class WebTestFixture
{
    public virtual Guid UserId => Guid.NewGuid();

    public Mock<IMediator> MediatorMock { get; }
    public IMapper Mapper { get; }

    public WebTestFixture()
    {
        MediatorMock = new(MockBehavior.Strict);
        Mapper = new MapperConfiguration(ApplyMappingProfiles).CreateMapper();
    }

    protected virtual void ApplyMappingProfiles(IMapperConfigurationExpression cfg)
    {

    }

    [SetUp]
    public virtual void BeforeEachTest()
    {
        MediatorMock.Invocations.Clear();
        IdentityState.SetCurrent(UserId, null);
    }
}
