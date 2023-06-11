using System.Security.Claims;

namespace IvNav.Store.Common.Identity;

public class IdentityState
{
    private static readonly AsyncLocal<IdentityState?> CurrentLocal = new();

    /// <summary>
    /// Gets or sets the current operation (IdentityState) for the current thread.
    /// This flows across async calls.
    /// </summary>
    public static IdentityState? Current => CurrentLocal.Value;

    public static void SetCurrent(IEnumerable<Claim> claims)
    {
        var userIdString = claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)?.Value;

        if (Guid.TryParse(userIdString, out var userId))
        {
            CurrentLocal.Value = new IdentityState(userId);
        }
    }

    public Guid UserId { get; }

    public Guid? TenantId { get; }

    private IdentityState(Guid userId)
    {
        UserId = userId;
        TenantId = null;
    }
}
