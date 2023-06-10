using AutoMapper;
using IvNav.Store.Product.Web.Helpers.Mapper;
using IvNav.Store.Testing.Helpers;

namespace IvNav.Store.Product.Web.Tests;

internal class ProductWebTestFixture : WebTestFixture
{
    protected override void ApplyMappingProfiles(IMapperConfigurationExpression cfg)
    {
        cfg.AddProfile<WebAutoMapperProfile>();
    }
}
