using AutoMapper;
using IvNav.Store.Identity.Core.Models.User;
using IvNav.Store.Identity.Web.Models;

namespace IvNav.Store.Identity.Web.Helpers.Mapper;

internal class WebAutoMapperProfile : Profile
{
    public WebAutoMapperProfile()
    {
        CreateMap<UserModel, UserInfoResponseDto>();
    }
}
