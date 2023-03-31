namespace IvNav.Store.Common.Identity;

public class IdentityState
{
    private static readonly AsyncLocal<IdentityState?> CurrentLocal = new();

    /// <summary>
    /// Gets or sets the current operation (IdentityState) for the current thread.
    /// This flows across async calls.
    /// </summary>
    public static IdentityState? Current => CurrentLocal.Value;

    public static void SetCurrent(Guid tenantId, Guid userId)
    {
        CurrentLocal.Value = new IdentityState(tenantId, userId);
    }

    public Guid TenantId { get; }

    public Guid UserId { get; set; }

    private IdentityState(Guid tenantId, Guid userId)
    {
        TenantId = tenantId;
    }
}
