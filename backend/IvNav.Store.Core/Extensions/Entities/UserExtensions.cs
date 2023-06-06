using IvNav.Store.Core.Models.User;

namespace IvNav.Store.Core.Extensions.Entities;

internal static class UserExtensions
{
    internal static UserModel MapToModel(this Infrastructure.Entities.Identity.User entity)
    {
        return new UserModel
        {
            Id = entity.Id,
            GivenName = entity.GivenName,
            Surname = entity.Surname,
            DateOfBirth = entity.DateOfBirth,
            Phone = entity.Phone,
            NeedSetupPassword = entity.NeedSetupPassword,
        };
    }
}
