namespace IvNav.Store.Core.Extensions.Common;

internal static class DictionaryExtensions
{
    public static void Upsert(this IDictionary<string, List<string>> dictionary, string key, string value)
    {
        if (!dictionary.ContainsKey(key))
        {
            dictionary.Add(key, new List<string>());
        }

        if (dictionary[key].All(i => i != value))
        {
            dictionary[key].Add(value);
        }
    }
}
