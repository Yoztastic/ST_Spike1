namespace StorageSpike.Host.Middleware.HttpContextTracing.Implementation;

internal static class DictionaryExtensions
{
    internal static Dictionary<string, string> ExcludeStripNullValue(this Dictionary<string, string?> dictionary, ICollection<string> collection) =>
        Exclude(new Dictionary<string, string>(WithoutNulValues(dictionary)), collection);

    private static IEnumerable<KeyValuePair<string, string>> WithoutNulValues(Dictionary<string, string?> dictionary) =>
        dictionary.Where(kvp => kvp.Value != null)
            .Select(kvp => new KeyValuePair<string, string>(kvp.Key, kvp.Value!));

    internal static Dictionary<string, string> Exclude(this Dictionary<string, string> dictionary,
        ICollection<string> collection) =>
        new(dictionary.Where(kvp => !collection.Contains(kvp.Key)));

    internal static Dictionary<string, string> Mask(this Dictionary<string, string> dictionary, ICollection<string> collection) =>
        new(dictionary.Select(kvp => collection.Contains(kvp.Key)? new KeyValuePair<string, string>(kvp.Key,"***"):kvp));
}
