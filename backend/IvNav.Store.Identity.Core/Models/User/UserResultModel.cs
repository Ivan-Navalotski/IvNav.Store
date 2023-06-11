using Ardalis.GuardClauses;

namespace IvNav.Store.Identity.Core.Models.User;

internal class UserResultModel
{
    public Infrastructure.Entities.User? User { get; }
    public bool Succeeded { get; }

    public IReadOnlyDictionary<string, string[]> Errors { get; }

    internal UserResultModel(Dictionary<string, List<string>> errors)
    {
        Succeeded = false;
        Errors = Guard.Against.Null(errors).ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value.ToArray());
    }

    internal UserResultModel(Infrastructure.Entities.User user)
    {
        Succeeded = true;
        Errors = new Dictionary<string, string[]>();
        User = user;
    }
}
