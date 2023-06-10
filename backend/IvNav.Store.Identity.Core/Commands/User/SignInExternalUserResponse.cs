using System.Security.Claims;

namespace IvNav.Store.Identity.Core.Commands.User;

public class SignInExternalUserResponse
{
    public bool Succeeded { get; }

    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public IReadOnlyCollection<Claim>? Claims { get; }

    internal SignInExternalUserResponse(IReadOnlyDictionary<string, string[]> errors)
    {
        Succeeded = false;
        Errors = errors.ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value.ToArray());
    }

    internal SignInExternalUserResponse(IReadOnlyCollection<Claim>? claims)
    {
        Succeeded = true;
        Errors = new Dictionary<string, string[]>();
        Claims = claims;
    }
}
