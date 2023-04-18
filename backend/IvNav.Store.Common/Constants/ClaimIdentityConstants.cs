using System.Security.Claims;

namespace IvNav.Store.Common.Constants;

public static class ClaimIdentityConstants
{
    public static string UserIdClaimType => ClaimTypes.NameIdentifier;
    public static string TenantIdClaimType => "TenantId";
}
