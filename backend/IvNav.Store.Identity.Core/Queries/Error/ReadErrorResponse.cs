using Ardalis.GuardClauses;

namespace IvNav.Store.Identity.Core.Queries.Error;

public class ReadErrorResponse
{
    public string Error { get; }

    public string? Description { get; }

    public ReadErrorResponse(string error, string? description)
    {
        Error = Guard.Against.NullOrEmpty(error);
        Description = description;
    }
}
