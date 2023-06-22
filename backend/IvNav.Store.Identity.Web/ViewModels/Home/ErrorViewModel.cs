using Ardalis.GuardClauses;

namespace IvNav.Store.Identity.Web.ViewModels.Home;

public class ErrorViewModel
{
    public string Error { get; }

    public string? Description { get; init; }

    public ErrorViewModel(string error)
    {
        Error = Guard.Against.NullOrEmpty(error);
    }
}
