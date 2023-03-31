using System.Security.Claims;

namespace IvNav.Store.Common.Constants;

public static class IdentityConstants
{
    public static string UserIdClaimType => ClaimTypes.NameIdentifier;
    public static string TenantIdClaimType => "TenantId";
}
