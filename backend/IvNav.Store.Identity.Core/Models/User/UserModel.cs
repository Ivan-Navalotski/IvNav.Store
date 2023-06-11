namespace IvNav.Store.Identity.Core.Models.User;

public class UserModel
{
    public Guid Id { get; init; }

    public bool NeedSetupPassword { get; init; }

    public string? GivenName { get; init; }

    public string? Surname { get; init; }

    public string? MobilePhone { get; init; }
}
