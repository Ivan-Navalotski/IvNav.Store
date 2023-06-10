using Ardalis.GuardClauses;

namespace IvNav.Store.Core.Models.User;

internal class UserManagerReultModel
{
    public Guid? UserId { get; }
    public bool Succeeded { get; }

    public IReadOnlyDictionary<string, string[]> Errors { get; }

    internal UserManagerReultModel(Dictionary<string, List<string>> errors)
    {
        Succeeded = false;
        Errors = Guard.Against.Null(errors).ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value.ToArray());
    }

    internal UserManagerReultModel(Guid userId)
    {
        Succeeded = true;
        Errors = new Dictionary<string, string[]>();
        UserId = Guard.Against.Default(userId);
    }
}
