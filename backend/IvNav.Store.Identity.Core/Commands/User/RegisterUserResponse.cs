using Ardalis.GuardClauses;

namespace IvNav.Store.Identity.Core.Commands.User;

public class RegisterUserResponse
{
    public bool Succeeded { get; private init; }

    public IReadOnlyDictionary<string, string[]> Errors { get; private init; }

    private RegisterUserResponse()
    {
        Errors = new Dictionary<string, string[]>();
    }

    internal static RegisterUserResponse Error(IReadOnlyDictionary<string, string[]> errors)
    {
        Guard.Against.NullOrEmpty(errors);

        return new RegisterUserResponse
        {
            Succeeded = false,
            Errors = errors.ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value.ToArray()),
        };
    }

    internal static RegisterUserResponse Success()
    {
        return new RegisterUserResponse
        {
            Succeeded = true,
        };
    }
}
