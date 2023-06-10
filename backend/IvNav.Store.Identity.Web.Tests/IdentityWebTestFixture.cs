using AutoMapper;
using IvNav.Store.Identity.Web.Helpers.Mapper;
using IvNav.Store.Testing.Helpers;

namespace IvNav.Store.Identity.Web.Tests;

internal class IdentityWebTestFixture : WebTestFixture
{
    protected override void ApplyMappingProfiles(IMapperConfigurationExpression cfg)
    {
        cfg.AddProfile<WebAutoMapperProfile>();
    }
}
