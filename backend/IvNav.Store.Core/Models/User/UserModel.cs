namespace IvNav.Store.Core.Models.User;

public class UserModel
{
    public Guid Id { get; init; }

    public string? GivenName { get; init; }

    public string? Surname { get; init; }

    public DateOnly? DateOfBirth { get; init; }

    public string? Phone { get; init; }

    public bool NeedSetupPassword { get; init; }
}
