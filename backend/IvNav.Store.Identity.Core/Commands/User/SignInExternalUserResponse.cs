using Ardalis.GuardClauses;

namespace IvNav.Store.Identity.Core.Commands.User;

public class SignInExternalUserResponse
{
    public bool Succeeded { get; private init; }

    public IReadOnlyDictionary<string, string[]> Errors { get; private init; }

    public Guid UserId { get; set; }

    private SignInExternalUserResponse()
    {
        Errors = new Dictionary<string, string[]>();
    }

    internal static SignInExternalUserResponse Error(IReadOnlyDictionary<string, string[]> errors)
    {
        Guard.Against.NullOrEmpty(errors);

        return new SignInExternalUserResponse
        {
            Succeeded = false,
            Errors = errors.ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value.ToArray()),
        };
    }

    internal static SignInExternalUserResponse Success(Guid userId)
    {
        return new SignInExternalUserResponse
        {
            Succeeded = true,
            UserId = userId,
        };
    }
}
