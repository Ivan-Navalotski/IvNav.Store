namespace IvNav.Store.Identity.Core.Commands.User;

public class SignInExternalUserResponse
{
    public bool Succeeded { get; }

    public IReadOnlyDictionary<string, string[]> Errors { get; }

    internal SignInExternalUserResponse(IReadOnlyDictionary<string, string[]> errors)
    {
        Succeeded = false;
        Errors = errors.ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value.ToArray());
    }

    internal SignInExternalUserResponse()
    {
        Succeeded = true;
        Errors = new Dictionary<string, string[]>();
    }
}
