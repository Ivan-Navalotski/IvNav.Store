namespace IvNav.Store.Web.Models.V1.User;

/// <summary>
/// User information.
/// </summary>
public sealed class UserInfoResponseDto
{
    public string? GivenName { get; init; }

    public string? Surname { get; init; }

    public DateOnly? DateOfBirth { get; init; }

    public string? Phone { get; init; }

    /// <summary>
    /// The user registered from an external provider and hasn't set up a password yet.
    /// </summary>
    public bool NeedSetupPassword { get; init; }
}
