using AutoMapper;
using IvNav.Store.Core.Models.Product;
using IvNav.Store.Web.Models.V1.Product;

namespace IvNav.Store.Web.Helpers.Mapper;

internal class WebAutoMapperProfile : Profile
{
    public WebAutoMapperProfile()
    {
        CreateMap<ProductModel, ReadProductResponseDto>();
    }
}
