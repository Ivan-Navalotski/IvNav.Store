using Ardalis.GuardClauses;

namespace IvNav.Store.Core.Commands.User;

public class RegisterUserResponse
{
    public bool Succeeded => !Errors.Any();

    public IReadOnlyDictionary<string, string[]> Errors { get; }

    internal RegisterUserResponse(Dictionary<string, List<string>> errors)
    {
        Errors = Guard.Against.Null(errors).ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value.ToArray());
    }
}
