namespace IvNav.Store.Common.Extensions;

public static class EnumExtensions
{
    public static T[] GetAttributes<T>(this Enum value)
    {
        var fi = value.GetType().GetField(value.ToString());
        if (fi == null) return Array.Empty<T>();

        if (fi.GetCustomAttributes(typeof(T), false) is T[] attributes)
        {
            return attributes;
        }
        return Array.Empty<T>();
    }
}
