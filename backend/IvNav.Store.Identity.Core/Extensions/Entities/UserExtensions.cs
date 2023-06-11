using IvNav.Store.Identity.Core.Models.User;
using IvNav.Store.Identity.Infrastructure.Entities;

namespace IvNav.Store.Identity.Core.Extensions.Entities;

internal static class UserExtensions
{
    internal static UserModel MapToModel(this User entity)
    {
        return new UserModel
        {
            Id = entity.Id,
            NeedSetupPassword = entity.NeedSetupPassword,
        };
    }
}
