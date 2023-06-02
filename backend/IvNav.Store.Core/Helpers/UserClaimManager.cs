using IvNav.Store.Common.Constants;
using System.Security.Claims;

namespace IvNav.Store.Core.Helpers;

internal class UserClaimManager
{
    public IReadOnlyList<Claim> GetUserClaims(Guid userId)
    {
        var claims = new[]
        {
            new Claim(ClaimIdentityConstants.UserIdClaimType, userId.ToString()),
            new Claim(ClaimIdentityConstants.TenantIdClaimType, Guid.NewGuid().ToString()),
        };

        return claims;
    }
}
