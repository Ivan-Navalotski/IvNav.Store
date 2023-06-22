using Ardalis.GuardClauses;

namespace IvNav.Store.Identity.Core.Commands.User;

public class SignInUserResponse
{
    public bool Succeeded { get; }

    public IReadOnlyDictionary<string, string[]> Errors { get; }

    internal SignInUserResponse(IReadOnlyDictionary<string, string[]> errors)
    {
        Succeeded = false;
        Errors = Guard.Against.Null(errors);
    }

    internal SignInUserResponse()
    {
        Succeeded = true;
        Errors = new Dictionary<string, string[]>();
    }
}
