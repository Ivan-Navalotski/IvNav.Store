using IvNav.Store.Common.Extensions;
using static IvNav.Store.Identity.Core.Enums.UserErrors;

namespace IvNav.Store.Identity.Core.Extensions;

internal static class DictionaryExtensions
{
    public static void AddUserError(this IDictionary<string, List<string>> dictionary, ExternalIdError error)
    {
        dictionary.Upsert("ExternalId", error.ToString());
    }

    public static void AddUserError(this IDictionary<string, List<string>> dictionary, EmailError error)
    {
        dictionary.Upsert("Email", error.ToString());
    }

    public static void AddUserError(this IDictionary<string, List<string>> dictionary, PasswordError error)
    {
        dictionary.Upsert("Password", error.ToString());
    }

    public static void AddUserError(this IDictionary<string, List<string>> dictionary, ReturnUrlError error)
    {
        dictionary.Upsert("ReturnUrl", error.ToString());
    }
}
