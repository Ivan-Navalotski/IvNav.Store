using Ardalis.GuardClauses;

namespace IvNav.Store.Identity.Core.Models.User;

internal class ValidateReturnUrlResultModel
{
    public bool Succeeded { get; }

    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public bool IsLocalUrl { get; }

    internal ValidateReturnUrlResultModel(Dictionary<string, List<string>> errors, bool isLocalUrl)
    {
        Succeeded = false;
        Errors = Guard.Against.Null(errors).ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value.ToArray());
        IsLocalUrl = isLocalUrl;
    }

    internal ValidateReturnUrlResultModel(bool isLocalUrl)
    {
        Succeeded = true;
        Errors = new Dictionary<string, string[]>();
        IsLocalUrl = isLocalUrl;
    }
}
