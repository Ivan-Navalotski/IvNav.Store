using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IvNav.Store.Identity.Web.Extensions;

internal static class ModelStateDictionaryExtensions
{
    public static void AddErrors(this ModelStateDictionary modelState, IReadOnlyDictionary<string, string[]> errors)
    {
        foreach (var keyValuePair in errors)
        {
            foreach (var error in keyValuePair.Value)
            {
                modelState.AddModelError(keyValuePair.Key, error);
            }
        }
    }
}
