using AutoMapper;
using IvNav.Store.Product.Core.Models.Product;
using IvNav.Store.Product.Web.Models.V1.Product;

namespace IvNav.Store.Product.Web.Helpers.Mapper;

internal class WebAutoMapperProfile : Profile
{
    public WebAutoMapperProfile()
    {
        CreateMap<ProductModel, ReadProductResponseDto>();
    }
}
