using AutoMapper;
using IvNav.Store.Common.Identity;
using MediatR;
using Moq;
using NUnit.Framework;
using System.Security.Claims;

namespace IvNav.Store.Testing.Helpers;

public class WebTestFixture
{
    private readonly Guid _userId = Guid.Parse("144ff3a4-2549-4708-9f74-e8273f85d138");
    public Guid UserId => _userId;

    public Mock<IMediator> MediatorMock { get; }
    public IMapper Mapper { get; }

    public WebTestFixture()
    {
        MediatorMock = new(MockBehavior.Strict);
        Mapper = new MapperConfiguration(ApplyMappingProfiles).CreateMapper();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, _userId.ToString()),
        };

        IdentityState.SetCurrent(claims);
    }

    protected virtual void ApplyMappingProfiles(IMapperConfigurationExpression cfg)
    {

    }

    [SetUp]
    public virtual void BeforeEachTest()
    {
        MediatorMock.Invocations.Clear();
    }
}
