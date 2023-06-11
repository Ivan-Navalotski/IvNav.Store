using Ardalis.GuardClauses;

namespace IvNav.Store.Identity.Core.Commands.User;

public class SignInUserResponse
{
    public bool Succeeded { get; }

    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public bool IsLocalUrl { get; }

    internal SignInUserResponse(IReadOnlyDictionary<string, string[]> errors)
    {
        Succeeded = false;
        Errors = Guard.Against.Null(errors);
    }

    internal SignInUserResponse(bool isLocalUrl)
    {
        Succeeded = true;
        Errors = new Dictionary<string, string[]>();
        IsLocalUrl = isLocalUrl;
    }
}
