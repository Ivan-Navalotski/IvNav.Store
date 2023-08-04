using Ardalis.GuardClauses;

namespace IvNav.Store.Identity.Core.Commands.User;

public class SignInUserResponse
{
    public bool Succeeded { get; private init; }

    public IReadOnlyDictionary<string, string[]> Errors { get; private init; }

    public Guid UserId { get; set; }

    private SignInUserResponse()
    {
        Errors = new Dictionary<string, string[]>();
    }

    internal static SignInUserResponse Error(IReadOnlyDictionary<string, string[]> errors)
    {
        Guard.Against.NullOrEmpty(errors);

        return new SignInUserResponse
        {
            Succeeded = false,
            Errors = errors.ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value.ToArray()),
        };
    }

    internal static SignInUserResponse Success(Guid userId)
    {
        return new SignInUserResponse
        {
            Succeeded = true,
            UserId = userId,
        };
    }
}
