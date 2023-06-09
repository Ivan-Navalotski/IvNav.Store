using IvNav.Store.Common.Extensions;
using IvNav.Store.Core.Interaction.Attributes;

namespace IvNav.Store.Core.Interaction.Extensions;

internal static class EnumExtensions
{
    public static string GetAppId(this Enum value)
    {
        var attributes = value.GetAttributes<AppIdAttribute>();
        return attributes.Any() ? attributes.First().Value : value.ToString();
    }
}
