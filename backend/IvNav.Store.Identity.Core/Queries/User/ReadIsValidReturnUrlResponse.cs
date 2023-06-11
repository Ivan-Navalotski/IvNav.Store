using Ardalis.GuardClauses;

namespace IvNav.Store.Identity.Core.Queries.User;

public class ReadIsValidReturnUrlResponse
{
    public bool Succeeded { get; }

    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public bool IsLocalUrl { get; }

    internal ReadIsValidReturnUrlResponse(
        IReadOnlyDictionary<string, string[]> errors, bool isLocalUrl)
    {
        Succeeded = false;
        Errors = Guard.Against.Null(errors);
        IsLocalUrl = isLocalUrl;
    }

    internal ReadIsValidReturnUrlResponse(bool isLocalUrl)
    {
        Succeeded = true;
        Errors = new Dictionary<string, string[]>();
        IsLocalUrl = isLocalUrl;
    }
}
