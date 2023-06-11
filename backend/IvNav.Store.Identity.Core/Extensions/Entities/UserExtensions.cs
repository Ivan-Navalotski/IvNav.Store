using System.Security.Claims;
using IvNav.Store.Identity.Core.Models.User;
using IvNav.Store.Identity.Infrastructure.Entities;
using static Duende.IdentityServer.Models.IdentityResources;

namespace IvNav.Store.Identity.Core.Extensions.Entities;

internal static class UserExtensions
{
    internal static UserModel MapToModel(this User entity, IList<Claim> claims)
    {
        return new UserModel
        {
            Id = entity.Id,
            NeedSetupPassword = entity.NeedSetupPassword,
            GivenName = claims.FirstOrDefault(i => i.Type == ClaimTypes.GivenName)?.Value,
            Surname = claims.FirstOrDefault(i => i.Type == ClaimTypes.Surname)?.Value,
            MobilePhone = claims.FirstOrDefault(i => i.Type == ClaimTypes.MobilePhone)?.Value,
        };
    }
}
