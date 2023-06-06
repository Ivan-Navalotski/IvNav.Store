using AutoMapper;
using IvNav.Store.Core.Models.Product;
using IvNav.Store.Core.Models.User;
using IvNav.Store.Web.Models.V1.Product;
using IvNav.Store.Web.Models.V1.User;

namespace IvNav.Store.Web.Helpers.Mapper;

internal class WebAutoMapperProfile : Profile
{
    public WebAutoMapperProfile()
    {
        CreateMap<ProductModel, ReadProductResponseDto>();
        CreateMap<UserModel, UserInfoResponseDto>();
    }
}
