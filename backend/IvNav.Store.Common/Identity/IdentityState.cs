namespace IvNav.Store.Common.Identity;

public class IdentityState
{
    private static readonly AsyncLocal<IdentityState?> CurrentLocal = new();

    /// <summary>
    /// Gets or sets the current operation (IdentityState) for the current thread.
    /// This flows across async calls.
    /// </summary>
    public static IdentityState? Current => CurrentLocal.Value;

    public static void SetCurrent(Guid userId, Guid? tenantId)
    {
        CurrentLocal.Value = new IdentityState(userId, tenantId);
    }

    public Guid UserId { get; set; }

    public Guid? TenantId { get; }

    private IdentityState(Guid userId, Guid? tenantId)
    {
        UserId = userId;
        TenantId = tenantId;
    }
}
