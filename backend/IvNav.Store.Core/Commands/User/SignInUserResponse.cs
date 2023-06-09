using Ardalis.GuardClauses;
using System.Security.Claims;

namespace IvNav.Store.Core.Commands.User;

public class SignInUserResponse
{
    public bool Succeeded => !Errors.Any();

    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public IReadOnlyCollection<Claim>? Claims { get; }

    internal SignInUserResponse(Dictionary<string, List<string>> errors)
    {
        Errors = Guard.Against.Null(errors).ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value.ToArray());
    }

    internal SignInUserResponse(IReadOnlyCollection<Claim>? claims)
    {
        Errors = new Dictionary<string, string[]>();
        Claims = claims;
    }
}
