using Ardalis.GuardClauses;

namespace IvNav.Store.Identity.Core.Commands.User;

public class RegisterUserResponse
{
    public bool Succeeded { get; }

    public IReadOnlyDictionary<string, string[]> Errors { get; }

    internal RegisterUserResponse(IReadOnlyDictionary<string, string[]> errors)
    {
        Succeeded = false;
        Errors = Guard.Against.Null(errors).ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value.ToArray());
    }

    internal RegisterUserResponse()
    {
        Errors = new Dictionary<string, string[]>();
        Succeeded = true;
    }
}
