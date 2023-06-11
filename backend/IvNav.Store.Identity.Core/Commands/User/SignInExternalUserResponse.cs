namespace IvNav.Store.Identity.Core.Commands.User;

public class SignInExternalUserResponse
{
    public bool Succeeded { get; }

    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public bool IsLocalUrl { get; }

    internal SignInExternalUserResponse(IReadOnlyDictionary<string, string[]> errors)
    {
        Succeeded = false;
        Errors = errors.ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value.ToArray());
    }

    internal SignInExternalUserResponse(bool isLocalUrl)
    {
        Succeeded = true;
        Errors = new Dictionary<string, string[]>();
        IsLocalUrl = isLocalUrl;
    }
}
